---
name: code-analysis
description: Analyze Contoso University architecture, dependencies, runtime behavior, CI, tests, or deployment using evidence from the current repository. Use for implementation questions and codebase investigations, not for changes that require another task-specific workflow.
---

# Code analysis

Inspect the relevant files before reaching conclusions. Do not infer implementation details from general .NET, EF Core, Docker, or microservices conventions when the repository can answer the question.

Identify the owning solution and executable, related libraries, configuration, runtime services, tests, CI workflows, and deployment assets that materially affect the question. Distinguish:

- project boundaries from runtime boundaries;
- runtime services from deployable units;
- intended architecture from current wiring;
- local Compose support from cloud deployment readiness.

For implementation-specific work, read the closest `AGENTS.md` and implementation `README.md`. Trace important claims to code or configuration rather than copying the README uncritically.

Report what was inspected, what the repository demonstrates, the supported conclusion, material uncertainty or inconsistency, and the next useful check if any.

If evidence is incomplete, say so explicitly. Never present a plausible convention as implemented behavior.

