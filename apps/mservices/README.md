# Contoso University — Microservices

This directory contains the microservices-style implementation of Contoso University.

It keeps the same broad university domain as the monolith, but splits the backend into separate API services and background workers. The MVC frontend communicates with backend services through typed HTTP clients, while cross-domain cleanup workflows are coordinated asynchronously with RabbitMQ and MassTransit.

This implementation is useful for practicing service boundaries, HTTP service-to-service communication, event-driven consistency, background workers, integration testing with external dependencies, and Docker Compose orchestration.

## Solution layout

```text
apps/mservices
├── ContosoUniversity.sln
├── docker-compose.yml
├── docker-compose.override.yml
├── src
│   ├── ContosoUniversity.Mvc
│   ├── ContosoUniversity.Application
│   ├── ContosoUniversity.ApiClients
│   ├── ContosoUniversity.Data
│   ├── ContosoUniversity.Messaging.Contracts
│   ├── ContosoUniversity.SharedKernel
│   ├── Courses.Api
│   ├── Courses.Core
│   ├── Courses.Data.Reads
│   ├── Courses.Data.Writes
│   ├── Courses.Worker
│   ├── Departments.Api
│   ├── Departments.Core
│   ├── Departments.Data.Reads
│   ├── Departments.Data.Writes
│   ├── Departments.Worker
│   ├── Students.Api
│   ├── Students.Core
│   ├── Students.Data.Reads
│   ├── Students.Data.Writes
│   └── Students.Worker
└── test
    ├── unit
    ├── integration
    ├── e2e
    └── system
```

## Architecture overview

The implementation consists of several runtime services:

- `ContosoUniversity.Mvc` — ASP.NET Core MVC frontend.
- `Courses.Api` — Minimal API for course read/write operations.
- `Departments.Api` — Minimal API for departments and instructors.
- `Students.Api` — Minimal API for student read/write operations.
- `Courses.Worker` — background consumer for department deletion events.
- `Departments.Worker` — background consumer for course deletion events affecting instructor assignments.
- `Students.Worker` — background consumer for course deletion events affecting student enrollments.

The shared projects provide infrastructure and contracts:

- `ContosoUniversity.ApiClients` — typed HTTP clients used by the MVC frontend.
- `ContosoUniversity.Messaging.Contracts` — RabbitMQ/MassTransit event contracts.
- `ContosoUniversity.Data` — shared EF Core / SQL Server infrastructure.
- `ContosoUniversity.SharedKernel` — shared domain building blocks.
- `*.Core` projects — domain and application behavior for each service area.
- `*.Data.Reads` and `*.Data.Writes` projects — read/write data access modules.

## Communication model

### Synchronous HTTP

The MVC frontend does not access the database directly. It registers typed HTTP clients and calls the backend APIs:

```text
ContosoUniversity.Mvc
  -> Courses.Api
  -> Departments.Api
  -> Students.Api
```

In Docker Compose, the frontend uses service DNS names:

```text
http://courses-api
http://departments-api
http://students-api
```

### Asynchronous messaging

RabbitMQ is used for cross-domain cleanup workflows.

Current events:

```text
course-deleted-event
department-deleted-event
```

Current event flow:

```text
Delete Department
  -> DepartmentDeletedEvent
  -> Courses.Worker removes related courses
  -> course deletion publishes CourseDeletedEvent
  -> Students.Worker withdraws enrolled students
  -> Departments.Worker resets instructor course assignments
```

This keeps cross-domain reactions out of the original HTTP request path and demonstrates eventual consistency in a small domain.

## Runtime dependencies

The local Docker Compose environment contains:

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

RabbitMQ uses the repository-level `rabbitmq.conf` and `definitions.json` files.

## API overview

### Courses API

Selected endpoints:

```text
GET    /api/courses
GET    /api/courses/{externalId}
GET    /api/courses/existsByCourseCode/{courseCode}
GET    /api/courses/title
POST   /api/courses
PUT    /api/courses/{externalId}
DELETE /api/courses/{externalId}
PUT    /api/courses/credits/update
```

### Departments API

Selected department endpoints:

```text
GET    /api/departments
GET    /api/departments/names
GET    /api/departments/{externalId}
POST   /api/departments
PUT    /api/departments/{externalId}
DELETE /api/departments/{externalId}
```

Selected instructor endpoints:

```text
GET    /api/instructors
GET    /api/instructors/names
GET    /api/instructors/{externalId}
POST   /api/instructors
PUT    /api/instructors/{externalId}
DELETE /api/instructors/{externalId}
```

### Students API

Selected endpoints:

```text
GET    /api/students/{externalId}
GET    /api/students/enrolled/groups
GET    /api/students/enrolled
POST   /api/students/search
POST   /api/students
PUT    /api/students/{externalId}
DELETE /api/students/{externalId}
```

## Local setup

### Prerequisites

- .NET SDK compatible with the solution target framework.
- Docker with Docker Compose support.
- Local developer HTTPS certificate tooling if HTTPS container access is needed.

### Prepare a development certificate

The MVC Dockerfile expects a certificate file at:

```text
apps/mservices/src/ContosoUniversity.Mvc/cert.pfx
```

Generate it with `dotnet dev-certs` from the MVC project directory. Use the same certificate password as the one configured for the local Docker Compose environment, or change the compose environment variable accordingly.

```bash
cd apps/mservices/src/ContosoUniversity.Mvc
dotnet dev-certs https -ep cert.pfx -p <local-dev-certificate-password>
```

### Start the local environment

From `apps/mservices`:

```bash
docker compose up --build --wait
```

The compose override maps the main services to the host:

```text
web                  http://localhost:10000, https://localhost:10001
courses-api           http://localhost:5006
departments-api       http://localhost:5079
students-api          http://localhost:5110
courses-worker        http://localhost:5025
departments-worker    http://localhost:5036
students-worker       http://localhost:5037
rabbitmq management   http://localhost:15672
mssql                 localhost,1477
```

### Stop the local environment

```bash
docker compose down
```

To remove volumes as well:

```bash
docker compose down -v
```

## Running tests

From `apps/mservices`:

```bash
dotnet restore
dotnet build --no-restore
dotnet test --no-build
```

The solution contains unit, integration, e2e, and system test projects. Integration coverage includes the MVC frontend, APIs, and workers. The test dependencies include SQL Server and RabbitMQ test containers, plus WireMock for HTTP boundary testing where needed.

## CI

The microservices PR-check workflow builds the solution, runs unit tests, validates whitespace/style/analyzer formatting with `dotnet format`, and runs integration tests for:

- `ContosoUniversity.Mvc`
- `Courses.Api`
- `Courses.Worker`
- `Departments.Api`
- `Departments.Worker`
- `Students.Api`
- `Students.Worker`

The workflow is scoped to changes under:

```text
apps/mservices/**
database/**
.github/**
```

## Deployment notes

This implementation currently has a complete Docker Compose runtime for local development and CI-level validation workflows.

Unlike the monolith implementation, this directory does not currently include a dedicated cloud deployment path such as Terraform infrastructure or GitHub Actions deployment workflows for ECS/Fargate. If such a deployment is added later, this README should be extended with environment topology, image publishing, infrastructure provisioning, service discovery, RabbitMQ hosting, database strategy, and rollout instructions.

## Notes

The microservices implementation intentionally keeps the domain small while introducing additional operational concerns: service boundaries, multiple runtime processes, broker-based messaging, event handlers, and consistency workflows that would be unnecessary in the monolith version but useful for architectural practice.
