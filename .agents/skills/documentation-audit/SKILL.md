---
name: documentation-audit
description: Audit repository documentation and agent guidance against the current code and configuration, then remediate only explicitly approved findings. Use for documentation drift audits, targeted fixes from an audit report, or verification of completed documentation-alignment work.
---

# Documentation Audit

Use one of these modes:

- `audit` performs a repository-wide read-only drift analysis and writes `.agent-reports/documentation-audit.md`. Read [references/audit-workflow.md](references/audit-workflow.md).
- `audit --changed <base>` scopes discovery to changes since the supplied Git branch or revision, while following related facts outside the diff when needed. Read [references/audit-workflow.md](references/audit-workflow.md).
- `fix <finding IDs>` remediates only the explicitly selected findings from the latest report. Read [references/fix-workflow.md](references/fix-workflow.md).
- `verify <finding IDs>` checks selected remediations without expanding their scope. Read the verification section in [references/fix-workflow.md](references/fix-workflow.md).

Examples:

```text
$documentation-audit audit
$documentation-audit audit --changed main
$documentation-audit fix DOC-P1-001 DOC-P2-003
$documentation-audit verify DOC-P1-001 DOC-P2-003
```

If the user omits the mode, default to `audit`. Treat writing the audit report as the only permitted repository-side effect in audit mode. Do not change implementation, configuration, or maintained documentation during an audit.

Read the root and applicable nested `AGENTS.md`, then use the repository skills relevant to the evidence or files being inspected. Treat code and executable configuration as evidence of implemented behavior, not automatically as proof of intended behavior.

Keep findings stable across modes. Each finding must have a unique ID, priority, evidence, recommended direction, and status. Never fix findings merely because they exist in the report: require the user to explicitly select their IDs or explicitly approve the entire report.

The report is a local handoff artifact and is ignored by Git. Update it after `fix` or `verify` so another agent or tool can resume the workflow without relying on chat history.
