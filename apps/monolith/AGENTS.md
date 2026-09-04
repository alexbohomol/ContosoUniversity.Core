# Monolith guidance

These instructions apply under `apps/monolith` and extend the repository-level `AGENTS.md`.

## Architecture invariants

- `ContosoUniversity.Mvc` is the single application entry point and deployable unit.
- The MVC process registers application and data modules directly. Do not describe data projects as runtime services.
- `ContosoUniversity.Application` owns application contracts, commands, queries, handlers, and validation behavior.
- `ContosoUniversity.Domain` owns domain entities and behavior.
- `ContosoUniversity.Data` provides shared EF Core and SQL Server infrastructure; `ContosoUniversity.Data.*.Reads` and `*.Writes` separate feature-oriented read/write access.
- Preserve the named read/write connection strings for Courses, Departments, and Students unless the task changes that design.

## Runtime and operations

- Local Compose services are `web`, `mssql`, `mssql-migrator`, and `aspire-dashboard`.
- The web application exposes `/health/readiness` and `/health/liveness`.
- The QA cloud path uses Terraform and AWS ECS/Fargate. SQL Server runs as an ECS container, not RDS.
- Deployment actions can affect shared infrastructure and require explicit user authorization.

## Verification

- Domain changes: `test/unit/ContosoUniversity.Domain.Tests`.
- MVC or database wiring: `test/integration/ContosoUniversity.Mvc.IntegrationTests`.
- Browser workflows: `test/e2e/ContosoUniversity.AcceptanceTests` when needed.
- Full runtime health: `test/system/ContosoUniversity.SystemTests` or the relevant Compose workflow when needed.

Read `README.md` for the maintained topology, configuration, local setup, CI, and deployment overview.

