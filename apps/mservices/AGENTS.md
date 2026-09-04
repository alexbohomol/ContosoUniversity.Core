# Microservices guidance

These instructions apply under `apps/mservices` and extend the repository-level `AGENTS.md`.

## Architecture invariants

- `ContosoUniversity.Mvc` is a frontend. It communicates with backend services through typed clients in `ContosoUniversity.ApiClients` and must not depend directly on EF Core data modules.
- The runtime APIs are `Courses.Api`, `Departments.Api`, and `Students.Api`.
- The runtime consumers are `Courses.Worker`, `Departments.Worker`, and `Students.Worker`.
- Messaging contracts belong in `ContosoUniversity.Messaging.Contracts`.
- Cross-domain cleanup currently uses RabbitMQ and MassTransit events. Preserve contract compatibility and verify all affected publishers and consumers when changing an event.
- Do not introduce a synchronous database dependency across service boundaries without an explicit architectural decision.

## Runtime and operations

- Docker Compose is the canonical local runtime and includes the frontend, three APIs, three workers, SQL Server, the migrator, and RabbitMQ.
- The repository does not currently contain a cloud deployment path equivalent to the monolith ECS/Fargate setup.

## Verification

- Domain behavior: the relevant unit-test project where one exists (currently `test/unit/Departments.Core.Tests`).
- API change: the matching `test/integration/*.Api.IntegrationTests` project.
- Worker or consumer change: the matching `test/integration/*.Worker.IntegrationTests` project.
- MVC or typed client change: `test/integration/ContosoUniversity.Mvc.IntegrationTests`.
- Cross-service browser workflow: `test/e2e/ContosoUniversity.AcceptanceTests` when needed.
- Full runtime health: `test/system/ContosoUniversity.SystemTests` or the relevant Compose workflow when needed.

Read `README.md` for the maintained service map, endpoints, event flow, local setup, and CI overview.
