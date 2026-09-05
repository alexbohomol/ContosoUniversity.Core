# Agent guidance for Contoso University

These instructions apply to the whole repository. User instructions take precedence. For work under an implementation directory, also read its local `AGENTS.md` and `README.md` before making changes.

## Repository map

- `apps/monolith` — modular monolith; solution: `apps/monolith/ContosoUniversity.slnx`.
- `apps/mservices` — microservices-style implementation; solution: `apps/mservices/ContosoUniversity.slnx`.
- `database` — shared SQL Server initialization and migration assets.
- `.github/workflows` — CI and monolith deployment workflows.
- `.agents/skills` — task-specific workflows loaded on demand.

The two applications implement similar domain behavior but are separate solutions. Do not change both implementations unless the task requires parity or the user explicitly requests it.

## Documentation ownership

- Code and configuration are the source of truth for implemented behavior.
- The root `README.md` owns repository-level orientation and links to implementation details.
- Implementation README files own current topology, setup, endpoints, CI, and deployment details.
- `AGENTS.md` files own durable repository rules and architectural constraints.
- Skills own repeatable task workflows, not copies of project inventories.
- `CLAUDE.md` and `.github/copilot-instructions.md` are compatibility adapters to the canonical `AGENTS.md` guidance.

When documentation and implementation disagree, verify the implementation and update the owning document rather than copying the correction into several files.

## Working agreements

1. Inspect the relevant implementation, tests, configuration, and infrastructure before making claims or edits.
2. Preserve existing architectural boundaries and naming. Treat intended architecture and current implementation as different things.
3. Keep changes focused. Preserve unrelated user changes in the worktree.
4. Use the matching repository skill for detailed testing, container, CI, deployment, analysis, or documentation workflows.
5. Do not hardcode secrets, credentials, tokens, certificate material, or machine-specific absolute paths.
6. Write committed documentation in English unless explicitly requested otherwise.
7. Before finishing, report the checks run, their results, and any checks that could not be run.

## Build entry points

Run solution commands from the corresponding implementation directory:

```bash
cd apps/monolith
dotnet restore
dotnet build --no-restore
```

```bash
cd apps/mservices
dotnet restore
dotnet build --no-restore
```

Consult the testing skill to select focused verification and determine whether broader suites and their runtime dependencies are relevant.

## Agent-documentation verification

Use Node.js 24 for local validation, matching the GitHub Actions workflow.

After changing agent guidance, compatibility adapters, implementation README files, or repository skills, run:

```bash
npm ci --prefix tools/agent
node tools/agent/validate-agent-docs.mjs
```

Detailed task workflows are discovered from `.agents/skills`; use the matching skill instead of duplicating its instructions here.
