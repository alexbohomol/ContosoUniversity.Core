import assert from "node:assert/strict";
import fs from "node:fs";
import os from "node:os";
import path from "node:path";
import test from "node:test";
import { createMarkdownLinkValidator } from "./markdown-links.mjs";

function fixture(t, content, files = {}) {
  const directory = fs.mkdtempSync(path.join(os.tmpdir(), "markdown-links-"));
  t.after(() => fs.rmSync(directory, { recursive: true, force: true }));
  const file = path.join(directory, "README.md");
  fs.writeFileSync(file, content);
  for (const [name, value] of Object.entries(files)) fs.writeFileSync(path.join(directory, name), value);
  return createMarkdownLinkValidator()(file, content);
}

test("checks same-document and cross-document fragments, preserving case", (t) => {
  assert.deepEqual(fixture(t, "# Setup\n[ok](#setup) [bad](#Setup) [ok](guide.md#install) [bad](guide.md#gone)", {
    "guide.md": "# Install",
  }), ["links to missing anchor #Setup.", "links to missing anchor guide.md#gone."]);
});

test("handles formatted headings, duplicate IDs, setext and Unicode", (t) => {
  assert.deepEqual(fixture(t, "# **Hello** `world`\n# Hello world\n# Привіт\nSection\n---\n[one](#hello-world) [two](#hello-world-1) [unicode](#%D0%BF%D1%80%D0%B8%D0%B2%D1%96%D1%82) [setext](#section)"), []);
});

test("supports HTML anchors and ignores comments and fenced code", (t) => {
  const content = '<a name="Legacy"></a>\n<div id="custom&amp;id"></div>\n<!-- <a id="hidden"></a> -->\n\n```md\n# Fake\n<a id="fake-html"></a>\n[ignored](missing.md)\n```\n\n[ok](#Legacy) [ok](#custom%26id) [bad](#fake) [bad](#fake-html) [bad](#hidden)';
  assert.deepEqual(fixture(t, content), ["links to missing anchor #fake.", "links to missing anchor #fake-html.", "links to missing anchor #hidden."]);
});

test("resolves reference-style links and images including later definitions", (t) => {
  assert.deepEqual(fixture(t, "[ok][target] [bad][missing] ![image][asset]\n\n[target]: guide.md#setup\n[missing]: guide.md#nope\n[asset]: absent.png", { "guide.md": "# Setup" }), [
    "links to missing anchor guide.md#nope.", "links to missing path absent.png.",
  ]);
});

test("splits before decoding and retains path-only validation", (t) => {
  assert.deepEqual(fixture(t, "[ok](guide%23one.md#setup) [ok](guide%23one.md) [bad](absent.md) [bad](#%ZZ) [ok](#) [external](https://example.com/#missing) [pdf](file.pdf#page=2)", {
    "guide#one.md": "# Setup", "file.pdf": "fixture",
  }), ["links to missing path absent.md.", "contains an invalid encoded link: #%ZZ."]);
});

test("frontmatter does not consume a heading slug", (t) => {
  assert.deepEqual(fixture(t, "---\nname: Setup\n---\n# Setup\n[ok](#setup) [bad](#name-setup)"), ["links to missing anchor #name-setup."]);
});

test("renaming a target heading invalidates the link on the next run", (t) => {
  assert.deepEqual(fixture(t, "[profile](guide.md#test-infrastructure-profiles)", { "guide.md": "## Renamed profiles" }), ["links to missing anchor guide.md#test-infrastructure-profiles."]);
});
