#!/usr/bin/env node

import fs from "node:fs";
import path from "node:path";
import process from "node:process";
import { fileURLToPath } from "node:url";

import { createMarkdownLinkValidator } from "./markdown-links.mjs";

const checkMarkdownLinks = createMarkdownLinkValidator();

const scriptDirectory = path.dirname(fileURLToPath(import.meta.url));
const repositoryRoot = path.resolve(scriptDirectory, "../..");
const errors = [];

const requiredFiles = [
  "AGENTS.md",
  "CLAUDE.md",
  ".github/copilot-instructions.md",
  "README.md",
  "apps/monolith/AGENTS.md",
  "apps/mservices/AGENTS.md",
  "apps/monolith/README.md",
  "apps/mservices/README.md",
  "apps/monolith/ContosoUniversity.slnx",
  "apps/mservices/ContosoUniversity.slnx",
  "apps/monolith/iac/envs/qa-aws-ecs",
];

function resolveFromRoot(relativePath) {
  return path.resolve(repositoryRoot, relativePath);
}

function relativeToRoot(filePath) {
  return path.relative(repositoryRoot, filePath).split(path.sep).join("/");
}

function fail(message) {
  errors.push(message);
}

function exists(relativePath) {
  return fs.existsSync(resolveFromRoot(relativePath));
}

function walk(directory, predicate = () => true) {
  const result = [];

  for (const entry of fs.readdirSync(directory, { withFileTypes: true })) {
    const entryPath = path.join(directory, entry.name);

    if (entry.isDirectory()) {
      result.push(...walk(entryPath, predicate));
    } else if (entry.isFile() && predicate(entryPath)) {
      result.push(entryPath);
    }
  }

  return result;
}

function parseSimpleFrontmatter(filePath, content) {
  const match = content.match(/^---\r?\n([\s\S]*?)\r?\n---(?:\r?\n|$)/);

  if (!match) {
    fail(`${relativeToRoot(filePath)} must start with YAML frontmatter.`);
    return {};
  }

  const metadata = {};

  for (const line of match[1].split(/\r?\n/)) {
    const property = line.match(/^([A-Za-z0-9_-]+):\s*(.+?)\s*$/);
    if (property) {
      metadata[property[1]] = property[2].replace(/^(["'])(.*)\1$/, "$2");
    }
  }

  return metadata;
}

function validateSkills() {
  const skillsRoot = resolveFromRoot(".agents/skills");

  if (!fs.existsSync(skillsRoot)) {
    fail("Missing .agents/skills directory.");
    return [];
  }

  const skillDirectories = fs
    .readdirSync(skillsRoot, { withFileTypes: true })
    .filter((entry) => entry.isDirectory())
    .sort((left, right) => left.name.localeCompare(right.name));

  if (skillDirectories.length === 0) {
    fail(".agents/skills must contain at least one skill directory.");
  }

  const skillFiles = [];

  for (const directory of skillDirectories) {
    const skillFile = path.join(skillsRoot, directory.name, "SKILL.md");
    skillFiles.push(skillFile);

    if (!fs.existsSync(skillFile)) {
      fail(`Missing .agents/skills/${directory.name}/SKILL.md.`);
      continue;
    }

    const metadata = parseSimpleFrontmatter(skillFile, fs.readFileSync(skillFile, "utf8"));

    if (!metadata.name) {
      fail(`${relativeToRoot(skillFile)} frontmatter must define name.`);
    } else {
      if (!/^[a-z0-9-]+$/.test(metadata.name)) {
        fail(`${relativeToRoot(skillFile)} name must contain only lowercase letters, digits, and hyphens.`);
      }

      if (metadata.name !== directory.name) {
        fail(`${relativeToRoot(skillFile)} name must match its directory (${directory.name}).`);
      }

      if (metadata.name.length > 64) {
        fail(`${relativeToRoot(skillFile)} name must not exceed 64 characters.`);
      }
    }

    if (!metadata.description?.trim()) {
      fail(`${relativeToRoot(skillFile)} frontmatter must define a non-empty description.`);
    }
  }

  return skillFiles.filter(fs.existsSync);
}

function validateImports(filePath, content) {
  for (const match of content.matchAll(/^\s*@([^\s]+)\s*$/gm)) {
    const importedPath = match[1].split("#", 1)[0];
    const resolvedPath = path.resolve(path.dirname(filePath), importedPath);

    if (!fs.existsSync(resolvedPath)) {
      fail(`${relativeToRoot(filePath)} imports missing file ${match[1]}.`);
    }
  }
}

function validateKnownContent(documentFiles) {
  const forbiddenPatterns = [
    [/(?:\/Users\/|\/home\/)/g, "do not commit Unix machine-specific absolute paths"],
    [/(?:\b[A-Za-z]:[\\/]|\\\\[^\\/\s]+[\\/][^\\/\s]+)/g, "do not commit Windows machine-specific absolute paths"],
  ];

  for (const filePath of documentFiles) {
    const content = fs.readFileSync(filePath, "utf8");

    validateImports(filePath, content);
    for (const error of checkMarkdownLinks(filePath, content)) {
      fail(`${relativeToRoot(filePath)} ${error}`);
    }

    for (const [pattern, guidance] of forbiddenPatterns) {
      if (pattern.test(content)) {
        fail(`${relativeToRoot(filePath)}: ${guidance}.`);
      }
      pattern.lastIndex = 0;
    }
  }
}

for (const requiredFile of requiredFiles) {
  if (!exists(requiredFile)) {
    fail(`Missing required repository path ${requiredFile}.`);
  }
}

if (exists(".ai") && walk(resolveFromRoot(".ai")).length > 0) {
  fail("Legacy .ai directory still contains files; repository agent guidance belongs in AGENTS.md and .agents/skills.");
}

const skillFiles = validateSkills();
const skillDocumentationFiles = exists(".agents/skills")
  ? walk(resolveFromRoot(".agents/skills"), (file) => file.endsWith(".md"))
  : [];
const documentationFiles = [
  resolveFromRoot("AGENTS.md"),
  resolveFromRoot("CLAUDE.md"),
  resolveFromRoot(".github/copilot-instructions.md"),
  resolveFromRoot("README.md"),
  resolveFromRoot("apps/monolith/AGENTS.md"),
  resolveFromRoot("apps/monolith/README.md"),
  resolveFromRoot("apps/mservices/AGENTS.md"),
  resolveFromRoot("apps/mservices/README.md"),
  ...skillDocumentationFiles,
].filter(fs.existsSync);

validateKnownContent(documentationFiles);

if (errors.length > 0) {
  console.error(`Agent documentation validation failed with ${errors.length} error(s):`);
  for (const error of errors) {
    console.error(`- ${error}`);
  }
  process.exitCode = 1;
} else {
  console.log(`Agent documentation validation passed (${skillFiles.length} skills, ${documentationFiles.length} documents).`);
}
