# Microservices skill

Use this skill when working under `apps/mservices`.

## Overview

`apps/mservices` contains the microservices-style implementation of Contoso University.

The main solution is:

```text
apps/mservices/ContosoUniversity.sln
```

This implementation keeps an MVC frontend but moves backend responsibilities into separate APIs and workers.

## Runtime services

The local runtime contains:

- `web`
- `courses-api`
- `courses-worker`
- `departments-api`
- `departments-worker`
- `students-api`
- `students-worker`
- `mssql`
- `mssql-migrator`
- `rabbitmq`

## Frontend boundary

`ContosoUniversity.Mvc` is the frontend.

It should communicate with backend services through typed HTTP clients from:

```text
ContosoUniversity.ApiClients
```

The MVC frontend should not directly depend on EF Core data modules.

Do not add direct database access to the MVC frontend unless the architectural decision is explicitly changed.

## APIs

Backend APIs are implemented as Minimal APIs:

```text
Courses.Api
Departments.Api
Students.Api
```

Read operations usually use read repositories.

Write operations usually go through MediatR commands.

## Workers

Background workers consume RabbitMQ/MassTransit events:

```text
Courses.Worker
Departments.Worker
Students.Worker
```

Workers are responsible for asynchronous cross-domain cleanup workflows.

## Messaging

Messaging contracts live in:

```text
ContosoUniversity.Messaging.Contracts
```

Current events include:

```text
course-deleted-event
department-deleted-event
```

Current flow:

```text
Delete Department
  -> DepartmentDeletedEvent
  -> Courses.Worker removes related courses
  -> course deletion publishes CourseDeletedEvent
  -> Students.Worker withdraws enrolled students
  -> Departments.Worker resets instructor course assignments
```

When adding new cross-domain behavior, consider whether it belongs in:

- synchronous API composition;
- asynchronous event publication;
- worker consumer;
- shared contract.

## Docker Compose

The local Docker Compose environment is the canonical local runtime.

Typical startup:

```bash
cd apps/mservices
docker compose up --build --wait
```

The MVC frontend uses service DNS names in Docker Compose:

```text
http://courses-api
http://departments-api
http://students-api
```

## Deployment

Do not claim that this implementation has the same AWS ECS/Fargate deployment path as the monolith.

As of the current repository state, the microservices implementation has local Docker Compose runtime and CI validation, but no dedicated cloud deployment workflow or Terraform environment equivalent to `apps/monolith/iac/envs/qa-aws-ecs`.

## Testing

Integration tests exist for:

- MVC frontend;
- APIs;
- workers.

Use RabbitMQ and SQL Server test infrastructure where needed.

Use WireMock or equivalent HTTP boundary testing when testing HTTP clients or service-to-service calls.
