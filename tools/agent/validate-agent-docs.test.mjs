import assert from "node:assert/strict";
import { spawnSync } from "node:child_process";
import fs from "node:fs";
import os from "node:os";
import path from "node:path";
import test from "node:test";
import { fileURLToPath } from "node:url";

const toolingDirectory = path.dirname(fileURLToPath(import.meta.url));
const skillPath = ".agents/skills/example/SKILL.md";
const validSkill = "---\nname: example\ndescription: Example workflow.\n---\n# Example\n";

function fixture(t) {
  const temporaryDirectory = fs.mkdtempSync(path.join(os.tmpdir(), "agent-docs-"));
  const root = path.join(temporaryDirectory, "repo");
  t.after(() => fs.rmSync(temporaryDirectory, { recursive: true, force: true }));
  function write(relativePath, content) {
    const target = path.join(root, relativePath);
    fs.mkdirSync(path.dirname(target), { recursive: true });
    fs.writeFileSync(target, content);
  }
  for (const file of [
    "AGENTS.md", "README.md", "apps/monolith/AGENTS.md", "apps/monolith/README.md",
    "apps/mservices/AGENTS.md", "apps/mservices/README.md",
  ]) write(file, "# Guidance\n");
  write("CLAUDE.md", "@AGENTS.md\n");
  write(".github/copilot-instructions.md", "@../AGENTS.md\n");
  write(skillPath, validSkill);

  // Copy the actual entry point so its repository root is the isolated fixture.
  // Reuse installed dependencies without copying or modifying the real checkout.
  for (const file of ["validate-agent-docs.mjs", "markdown-links.mjs"]) {
    write(`tools/agent/${file}`, fs.readFileSync(path.join(toolingDirectory, file), "utf8"));
  }
  fs.symlinkSync(path.join(toolingDirectory, "node_modules"), path.join(root, "tools/agent/node_modules"), "junction");
  return {
    write,
    remove: (relativePath) => fs.rmSync(path.join(root, relativePath)),
    run: () => {
      const result = spawnSync(process.execPath, [path.join(root, "tools/agent/validate-agent-docs.mjs")], {
        cwd: root, encoding: "utf8", timeout: 15_000,
      });
      assert.ifError(result.error);
      assert.equal(result.signal, null);
      return result;
    },
  };
}

function assertFailure(result, diagnostic) {
  assert.equal(result.status, 1, result.stdout + result.stderr);
  assert.equal(result.stdout, "");
  assert.match(result.stderr, /Agent documentation validation failed/);
  assert.ok(result.stderr.includes(diagnostic), result.stderr);
}

test("validator accepts a complete documentation fixture with relative imports", (t) => {
  const result = fixture(t).run();
  assert.equal(result.status, 0, result.stderr);
  assert.equal(result.stderr, "");
  assert.match(result.stdout, /Agent documentation validation passed/);
});

test("validator rejects missing skill frontmatter", (t) => {
  const repo = fixture(t);
  repo.write(skillPath, "# Example\n");
  assertFailure(repo.run(), `${skillPath} must start with YAML frontmatter.`);
});

test("validator rejects incomplete skill metadata", (t) => {
  const repo = fixture(t);
  repo.write(skillPath, "---\nname: example\n---\n# Example\n");
  assertFailure(repo.run(), `${skillPath} frontmatter must define a non-empty description.`);
});

test("validator rejects missing imports", (t) => {
  const repo = fixture(t);
  repo.write("CLAUDE.md", "@missing.md\n");
  assertFailure(repo.run(), "CLAUDE.md imports missing file missing.md.");
});

test("validator rejects a missing required document without inbound links", (t) => {
  const repo = fixture(t);
  repo.remove("README.md");
  assertFailure(repo.run(), "Missing required repository path README.md.");
});

for (const [platform, content] of [
  ["Unix", "/home/example/project"],
  ["Windows", "C:\\example\\project"],
]) {
  test(`validator rejects ${platform} machine-specific paths`, (t) => {
    const repo = fixture(t);
    repo.write("README.md", `# Setup\n${content}\n`);
    assertFailure(repo.run(), `README.md: do not commit ${platform} machine-specific absolute paths.`);
  });
}

test("validator propagates Markdown link failures to its exit status", (t) => {
  const repo = fixture(t);
  repo.write("README.md", "# Setup\n[broken](#missing)\n");
  assertFailure(repo.run(), "README.md links to missing anchor #missing.");
});

for (const [kind, importedPath] of [
  ["Unix", "/tmp/guidance.md"],
  ["Windows drive", "C:\\guidance.md"],
  ["UNC", "\\\\server\\share\\guidance.md"],
]) {
  test(`validator rejects absolute ${kind} imports`, (t) => {
    const repo = fixture(t);
    repo.write("CLAUDE.md", `@${importedPath}\n`);
    assertFailure(repo.run(), `CLAUDE.md imports an absolute path: ${importedPath}.`);
  });
}

for (const importedPath of ["../outside.md", "../repo-other/guidance.md"]) {
  test(`validator rejects an existing import outside the checkout: ${importedPath}`, (t) => {
    const repo = fixture(t);
    repo.write(importedPath, "# External guidance\n");
    repo.write("CLAUDE.md", `@${importedPath}\n`);
    assertFailure(repo.run(), `CLAUDE.md imports a path outside the repository: ${importedPath}.`);
  });
}

test("validator allows normalized parent segments inside the checkout", (t) => {
  const repo = fixture(t);
  repo.write("CLAUDE.md", "@apps/monolith/../../AGENTS.md\n");
  const result = repo.run();
  assert.equal(result.status, 0, result.stderr);
  assert.equal(result.stderr, "");
  assert.match(result.stdout, /Agent documentation validation passed/);
});
