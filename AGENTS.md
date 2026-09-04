# Agent guidance for Contoso University

These instructions apply to the whole repository. User instructions take precedence. For work under an implementation directory, also read its local `AGENTS.md` and `README.md` before making changes.

## Repository map

- `apps/monolith` — modular monolith; solution: `apps/monolith/ContosoUniversity.slnx`.
- `apps/mservices` — microservices-style implementation; solution: `apps/mservices/ContosoUniversity.slnx`.
- `database` — shared SQL Server initialization and migration assets.
- `.github/workflows` — CI and monolith deployment workflows.
- `.agents/skills` — task-specific workflows loaded on demand.

The two applications implement similar domain behavior but are separate solutions. Do not change both implementations unless the task requires parity or the user explicitly requests it.

## Working agreements

1. Inspect the relevant implementation, tests, configuration, and infrastructure before making claims or edits.
2. Preserve existing architectural boundaries and naming. Treat intended architecture and current implementation as different things.
3. Keep changes focused. Preserve unrelated user changes in the worktree.
4. Add or update tests at the lowest level that proves the changed behavior. Run targeted checks before broader suites.
5. Do not edit generated `*.feature.cs` files; change the source `.feature` files or bindings.
6. Do not hardcode secrets, credentials, tokens, certificate material, or machine-specific absolute paths.
7. Write committed documentation in English unless explicitly requested otherwise.
8. Before finishing, report the checks run, their results, and any checks that could not be run.

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

Use the relevant test project for focused verification. A solution-wide `dotnet test --no-build` includes integration, system, and acceptance projects and may require Docker or other runtime dependencies; consult the testing skill before running it.

## Verification routing

- Domain behavior or validation: relevant unit tests.
- MVC, API, EF Core, HTTP client, worker, or messaging behavior: relevant integration tests.
- Browser-visible workflow: acceptance/e2e tests when lower-level coverage is insufficient.
- Docker or Compose: validate Compose configuration and build the affected service.
- GitHub Actions: inspect triggers, variables, working directories, permissions, and affected commands.
- Terraform or deployment: run non-destructive formatting and validation first; do not provision, deploy, stop, or destroy infrastructure without explicit authorization.
- Documentation or agent guidance: run `node tools/agent/validate-agent-docs.mjs` to verify skill structure, imports, local links, terminology, and selected architecture invariants against the repository.

## Repository skills

Use the matching skill from `.agents/skills` for detailed workflows:

- `code-analysis` — repository-backed architecture and implementation analysis.
- `testing` — select and run proportionate tests.
- `docker-compose` — change or validate local container orchestration.
- `github-actions` — create, change, or review workflows.
- `aws-ecs-deployment` — work with the monolith QA ECS/Fargate deployment.
- `documentation` — create or update repository documentation.
