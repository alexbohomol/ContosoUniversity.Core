# Claude Code guidance

@AGENTS.md

The imported `AGENTS.md` is the canonical repository guidance. When working under `apps/monolith` or `apps/mservices`, also read the closest implementation `AGENTS.md` and `README.md`.

Reusable workflows are maintained as open Agent Skills under `.agents/skills`. Read the matching `SKILL.md` when the task involves code analysis, testing, Docker Compose, GitHub Actions, AWS ECS deployment, or documentation.

Do not create a separate copy of these rules under `.claude` unless Claude Code requires a compatibility adapter; keep `AGENTS.md` and `.agents/skills` as the source of truth.
