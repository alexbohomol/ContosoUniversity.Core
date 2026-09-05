# Microservices guidance

These instructions apply under `apps/mservices` and extend the repository-level `AGENTS.md`.

## Architecture invariants

- `ContosoUniversity.Mvc` is a frontend. It communicates with backend services through typed clients in `ContosoUniversity.ApiClients` and must not depend directly on EF Core data modules.
- Messaging contracts belong in `ContosoUniversity.Messaging.Contracts`.
- Cross-domain cleanup currently uses RabbitMQ and MassTransit events. Preserve contract compatibility and verify all affected publishers and consumers when changing an event.
- Do not introduce a synchronous database dependency across service boundaries without an explicit architectural decision.

## Verification

Read [README.md](README.md) for the maintained service map, endpoints, event flow, local setup, and CI overview. Use the testing or Docker Compose skill to select checks for the affected area.
