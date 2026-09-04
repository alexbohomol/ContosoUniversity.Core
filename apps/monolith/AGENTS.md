# Monolith guidance

These instructions apply under `apps/monolith` and extend the repository-level `AGENTS.md`.

## Architecture invariants

- `ContosoUniversity.Mvc` is the single application entry point and deployable unit.
- The MVC process registers application and data modules directly. Do not describe data projects as runtime services.
- Preserve the named read/write connection strings for Courses, Departments, and Students unless the task changes that design.
- Deployment actions can affect shared infrastructure and require explicit user authorization.

## Verification

Read [README.md](README.md) for the maintained topology, configuration, local setup, CI, and deployment overview. Use the testing, Docker Compose, or AWS ECS deployment skill to select checks for the affected area.
