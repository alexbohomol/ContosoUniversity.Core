import fs from "node:fs";
import path from "node:path";
import { unified } from "unified";
import remarkParse from "remark-parse";
import remarkFrontmatter from "remark-frontmatter";
import remarkGfm from "remark-gfm";
import GithubSlugger from "github-slugger";
import { toString } from "mdast-util-to-string";
import { parseFragment } from "parse5";

const parser = unified().use(remarkParse).use(remarkFrontmatter).use(remarkGfm);

function visit(node, callback) {
  callback(node);
  for (const child of node.children ?? node.childNodes ?? []) visit(child, callback);
}

function parseDocument(content) {
  const tree = parser.parse(content);
  const anchors = new Set();
  const slugger = new GithubSlugger();
  const definitions = new Map();
  const links = [];
  visit(tree, (node) => {
    if (node.type === "heading") {
      anchors.add(slugger.slug(toString(node, { includeHtml: false })));
    }
    if (node.type === "html") {
      visit(parseFragment(node.value), (element) => {
        for (const attribute of element.attrs ?? []) {
          if (attribute.name === "id" || (element.tagName === "a" && attribute.name === "name")) {
            anchors.add(attribute.value);
          }
        }
      });
    }
    if (node.type === "definition" && !definitions.has(node.identifier)) {
      definitions.set(node.identifier, node.url);
    }
  });
  visit(tree, (node) => {
    if (node.type === "link" || node.type === "image") links.push(node.url);
    if (node.type === "linkReference" || node.type === "imageReference") {
      const url = definitions.get(node.identifier);
      if (url !== undefined) links.push(url);
    }
  });
  return { anchors, links };
}

// One cache per validation run; linked documents are parsed at most once.
export function createMarkdownLinkValidator() {
  const documents = new Map();
  function document(filePath, content) {
    if (!documents.has(filePath)) {
      documents.set(filePath, parseDocument(content ?? fs.readFileSync(filePath, "utf8")));
    }
    return documents.get(filePath);
  }
  return function validateMarkdownLinks(filePath, content) {
    filePath = path.resolve(filePath);
    const errors = [];
    for (const target of document(filePath, content).links) {
      if (/^(?:[a-z][a-z0-9+.-]*:|\/\/)/i.test(target)) continue;
      const separator = target.indexOf("#");
      let pathname = separator < 0 ? target : target.slice(0, separator);
      let fragment = separator < 0 ? "" : target.slice(separator + 1);
      // Strip the URL query before decoding so %3F remains part of a filename.
      pathname = pathname.split("?", 1)[0];
      try {
        pathname = decodeURIComponent(pathname);
        fragment = decodeURIComponent(fragment);
      } catch {
        errors.push(`contains an invalid encoded link: ${target}.`);
        continue;
      }
      const resolvedPath = pathname ? path.resolve(path.dirname(filePath), pathname) : filePath;
      if (!fs.existsSync(resolvedPath)) {
        errors.push(`links to missing path ${target}.`);
      } else if (fragment && /\.(?:md|markdown)$/i.test(resolvedPath) && fs.statSync(resolvedPath).isFile()) {
        if (!document(resolvedPath).anchors.has(fragment)) {
          errors.push(`links to missing anchor ${target}.`);
        }
      }
    }
    return errors;
  };
}
