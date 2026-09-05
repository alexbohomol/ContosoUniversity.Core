# Fix and verification workflow

Use the latest `.agent-reports/documentation-audit.md` as the handoff. If it is absent or does not contain the requested IDs, stop and request a fresh audit or the missing report; do not reconstruct approvals from assumptions.

## Fix

Before editing, revalidate every selected finding against the current worktree because the repository may have changed since the audit. Mark a finding `rejected` with evidence if it is stale or incorrect.

For valid selected findings:

- follow their approved remediation direction;
- change implementation or configuration only when the finding explicitly identifies it as the incorrect side;
- do not guess when an owner decision is required;
- preserve unrelated and user-authored worktree changes;
- keep root documentation, implementation README files, `AGENTS.md`, skills, and executable configuration within their documented ownership boundaries;
- replace duplicated facts with links to one maintained source where practical;
- avoid generated files, unsupported claims, machine-specific paths, and real secrets.

Use the applicable repository skills for files being changed. Run validation proportionate to the selected findings, including as relevant:

- `npm ci --prefix tools/agent`, then `node tools/agent/validate-agent-docs.mjs`;
- `git diff --check`;
- Markdown link, anchor, and referenced-path checks;
- configuration validation;
- project-scoped build, format, or tests according to the testing skill.

Do not run deployment, destructive cleanup, or a full Docker/test lifecycle unless it is required to prove an approved fix and is authorized by the user.

Review the final diff as a code review. Then update each selected finding in the report to `fixed`, `blocked`, or `rejected`, including changed files, checks, results, and remaining limitations. Do not mark a finding `verified` in the same step unless its acceptance criteria were independently demonstrated by the validation performed.

## Verify

For `verify <finding IDs>`, make no implementation or documentation edits. Inspect the current repository, rerun the checks named by each finding, and look for conflicts introduced by its remediation only within the affected area.

Set the finding to:

- `verified` when its mismatch is gone and its stated checks pass;
- `fixed` when the edit exists but verification remains incomplete;
- `open` when the mismatch remains or regressed;
- `blocked` when a required dependency or owner decision prevents verification.

Update the report with evidence and return a concise status summary. Never broaden verification into fixes without explicit authorization.
