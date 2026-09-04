# GitHub Copilot guidance

@AGENTS.md

The imported `AGENTS.md` is the canonical repository-wide instruction source. When working under `apps/monolith` or `apps/mservices`, also follow the closest implementation `AGENTS.md` and consult its `README.md`.

Reusable workflows are maintained in `.agents/skills`. Use the matching skill for code analysis, testing, Docker Compose, GitHub Actions, AWS ECS deployment, or documentation.

Do not duplicate repository rules in this file. Keep Copilot-specific compatibility guidance here and update the canonical files instead.
