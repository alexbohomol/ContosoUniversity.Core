# Docker Compose skill

Use this skill when working with `docker-compose.yml`, `docker-compose.override.yml`, Dockerfiles, container health checks, local infrastructure, or integration/e2e runtime setup.

## General principles

Docker Compose is used as the local multi-container runtime.

Preserve startup sequencing.

Prefer explicit health checks over timing-based waits.

Do not replace health checks with arbitrary sleeps.

## Health checks

Health checks should target container-internal ports, not host-mapped ports.

For ASP.NET Core containers, prefer:

```bash
curl --fail http://localhost:80/health/readiness || exit 1
```

inside the container.

Do not use host ports such as `10000` from inside the same container.

## depends_on

The repository intentionally uses Compose dependency conditions.

Use:

```yaml
condition: service_healthy
```

for long-running dependencies such as SQL Server, RabbitMQ, APIs, and workers.

Use:

```yaml
condition: service_completed_successfully
```

for one-off migrator/init containers.

## Monolith runtime

`apps/monolith/docker-compose.yml` includes:

- `web`
- `mssql`
- `mssql-migrator`
- `aspire-dashboard`

The web service depends on:

- healthy SQL Server;
- completed migrator.

## Microservices runtime

`apps/mservices/docker-compose.yml` includes:

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

The web service depends on healthy backend APIs and RabbitMQ.

APIs and workers depend on SQL Server, migrator completion, and RabbitMQ.

## Local startup

Typical local startup:

```bash
docker compose up --build --wait
```

Typical shutdown:

```bash
docker compose down
```

Remove volumes only when a clean database/broker state is needed:

```bash
docker compose down -v
```

## Certificate handling

Do not hardcode real secrets or certificate passwords in documentation or examples.

Use placeholders where possible:

```bash
dotnet dev-certs https -ep cert.pfx -p Test1234!
```

## Dockerfile changes

When changing Dockerfiles:

- preserve restore caching where possible;
- ensure all referenced projects are copied before `dotnet restore`;
- ensure runtime images contain tools required by health checks;
- check the corresponding Docker Compose build context and Dockerfile path.
