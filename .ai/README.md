# AI assistant guidance

This directory contains skill files for AI-assisted work on the Contoso University repository.

The goal is to make tools like GitHub Copilot, Codex, ChatGPT, and other coding agents behave consistently when analyzing, modifying, documenting, testing, or reviewing the repository.

## Skill files

- `skills/code-analysis.md` — how to inspect and reason about repository code.
- `skills/documentation.md` — documentation style and README rules.
- `skills/monolith.md` — guidance for `apps/monolith`.
- `skills/mservices.md` — guidance for `apps/mservices`.
- `skills/testing.md` — testing conventions and expectations.
- `skills/docker-compose.md` — Docker Compose and local runtime guidance.
- `skills/ci-github-actions.md` — GitHub Actions workflow guidance.
- `skills/deployment-aws-ecs.md` — monolith AWS ECS/Fargate deployment guidance.

## Core principle

All architectural and implementation conclusions must be based on the current repository code.

Do not assume behavior from framework conventions when the implementation can be inspected.
