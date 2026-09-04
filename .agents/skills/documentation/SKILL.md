---
name: documentation
description: Create, update, or review Contoso University README files, architecture notes, setup guides, deployment notes, ADRs, or pull request documentation. Use when committed documentation must remain accurate to the repository.
---

# Documentation

Write committed documentation in English unless the user explicitly requests otherwise. Prefer concise, practical, repository-specific language and relative repository links.

Verify statements against current code and configuration. In particular, check:

- `.slnx`, project, directory, workflow, and infrastructure paths;
- runtime service and deployable-unit boundaries;
- ports, endpoints, service names, commands, and prerequisites;
- test frameworks and package names;
- local versus cloud deployment capabilities.

Do not describe planned behavior as implemented or use unsupported claims such as "production-ready". Use placeholders for credentials, tokens, passwords, and certificate material.

Keep each fact in one maintained source when possible. Root documentation should orient readers and link to implementation details; implementation READMEs should own their topology and setup; `AGENTS.md` should own durable agent rules; skills should own repeatable workflows. Avoid copying large inventories between them.

After editing, check Markdown syntax, relative links, referenced paths, commands, and terminology. Update directory trees when structure changes.

