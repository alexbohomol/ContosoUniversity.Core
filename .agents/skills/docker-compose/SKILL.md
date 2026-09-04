---
name: docker-compose
description: Create, change, debug, or review Dockerfiles and Docker Compose runtime configuration for Contoso University. Use for service wiring, health checks, startup ordering, ports, volumes, certificates, and container builds.
---

# Docker Compose

Read the affected implementation's `AGENTS.md`, Compose files, Dockerfile, health endpoints, and dependent services before editing.

Preserve these repository conventions unless the task changes them:

- health checks use container-internal ports;
- long-running dependencies use `condition: service_healthy`;
- one-off migrators use `condition: service_completed_successfully`;
- startup correctness relies on health or completion signals, not arbitrary sleeps;
- Dockerfiles preserve restore-layer caching and copy every referenced project required by restore;
- certificate examples and environment values must not expose real secrets.

Validate configuration from the affected implementation directory:

```bash
docker compose config
```

Build or start only the affected services when that proves the change. Use `docker compose up --build --wait` for full local-runtime verification when proportionate. Do not run `docker compose down -v` unless the user requests a clean state or volume deletion is necessary and its data impact is clear.

Report affected services, validation commands, health results, and any runtime dependencies that were unavailable.

