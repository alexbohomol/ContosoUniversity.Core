---
name: github-actions
description: Create, change, debug, or review GitHub Actions workflows in this repository. Use for CI triggers, matrices, repository variables, permissions, build and test jobs, artifacts, or deployment automation.
---

# GitHub Actions

Inspect the workflow itself and every script, project, Compose file, or deployment asset it invokes. Verify repository paths rather than assuming them from job names.

Preserve separation between monolith and microservices workflows unless sharing is an explicit design choice. Check:

- event and path filters;
- `MONOLITH_SLN_PATH` versus `MSERVICES_SLN_PATH`;
- job permissions, secrets, repository variables, and environments;
- working directories and `.slnx` discovery;
- dependency order, matrices, artifacts, and failure handling;
- parity between documented and executed build, format, and test commands.

Do not hardcode or print credentials. Treat workflow changes that provision, deploy, stop, or destroy infrastructure as externally mutating operations requiring explicit authorization before execution.

Use local syntax or static validation tools when available. If a behavior can only be confirmed by a GitHub-hosted run, say so and identify the workflow/job that needs observation.

