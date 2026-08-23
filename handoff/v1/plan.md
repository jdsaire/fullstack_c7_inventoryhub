# Plan — v1

The build plan as approved before execution began, then amended once more mid-build — approved
again before any further code was written against the amendment — when the deployment scope was
cut. Neither approval edited the other's record; this file carries both, in the order they
happened.

---

# InventoryHub (C7 capstone) — build & deploy plan

## Context

This is the initial scaffold-through-feature build of InventoryHub, the Course 7 capstone: a
Blazor WebAssembly front-end (`ClientApp`) integrated with a .NET Minimal API back-end
(`ServerApp`), built as four graded Activities from the mandate doc, plus deployment wiring to
GitHub Pages (front-end) and Render (back-end). The target repo `jdsaire/fullstack_c7_inventoryhub`
currently holds only debris from an abandoned Azure Static Web App connection attempt made during
hosting evaluation — the wrong resource type for a project that needs a real ASP.NET Core Minimal
API back-end. That debris must be removed before real work begins, and the removal must be
explicitly approved here, not done unilaterally.

Per the repo owner's standing instruction, this build uses a named-branch + PR-opened-unmerged
flow from the very first real commit onward, work happens across five stop-and-review gates (four
Activity gates + one deployment gate), and every commit is one deliverable, attributed solely to
`jdsaire`.

## Preflight results (task 0 — complete)

- **gh CLI**: `~/bin/gh` v2.96.0, authenticated as `jdsaire` (keyring), scopes include `repo` and
  `workflow`.
- **Attachments**: both required files present and read in full — the mandate md (four Activities,
  literal starter code, 30-pt rubric) and the syllabus txt.
- **.NET SDK**: 10.0.201 installed; `dotnet new list` confirms `blazorwasm` and `webapi` templates
  are available for net10.0.
- **Repo state vs. `verified_state`**: confirmed via `gh api` — exactly two commits on `main`,
  matching exactly: `d8e3b0c` "Create demo" (adds `demo`) and `b17ea0c` "ci: add Azure Static Web
  Apps workflow file" (adds `.github/workflows/azure-static-web-apps-nice-meadow-0914cd610.yml`,
  with an `on-behalf-of: @Azure` trailer). Tree confirmed to hold only those two files — nothing
  else. State matches the prompt exactly; no drift.
- **Syllabus scope check**: Module 4 ("Using Copilot for Integrating Front-End and Back-End Code")
  maps 1:1 to the four graded Activities (integration code, debugging, JSON structuring,
  performance). Nothing in the syllabus expands this project's scope ceiling. No database, no
  auth, no SignalR required or implied for the capstone deliverable.

## Debris-cleanup decision (formal proposal, per hard_rules)

**Proposal**: reset `main` to a single orphan commit that removes `demo` and the
`azure-static-web-apps-*.yml` workflow entirely, replacing them with the real project scaffold
(the two-project solution). This is the *first* commit of the actual build, pushed directly to
`main` (the one and only direct-to-main push this run makes), establishing a base `main` can
receive a PR against. Everything after it lands on `deploy/v1-fullstack-build` inside a PR opened
against `main` and left unmerged.

This requires a **force-push** to `main` (history reset, not a revert). It is safe here because
both existing commits are confirmed, unrelated, auto-generated debris with no InventoryHub
content, and the repo has no other collaborators or branches to disrupt.

## Git identity

No `user.name`/`user.email` was configured anywhere on this machine. Resolved with GitHub's
standard privacy-preserving noreply address, set locally to this repo's clone only:

```
git config user.name "jdsaire"
git config user.email "88201583+jdsaire@users.noreply.github.com"
```

## PR mode

Confirmed: normal (open, not draft) — `gh pr create` against `main`, left unmerged throughout.

## Confirmed template output vs. `<architecture>` (differences found)

- **ClientApp**: `dotnet new blazorwasm` does not generate a `wwwroot/appsettings.json` by
  default — created fresh rather than edited. Placeholder demo pages (`Counter.razor`,
  `Weather.razor`, `sample-data/weather.json`) removed; `Home.razor` kept as a minimal landing
  page; `NavMenu.razor` updated to link to `/fetchproducts`.
- **ServerApp**: `dotnet new webapi` pulls in `Microsoft.AspNetCore.OpenApi` and a
  `/weatherforecast` sample. Since Program.cs is fully replaced with the mandate's literal
  `/api/products` starter handler, the now-unused OpenApi package reference was removed
  (a removal, not a new dependency) and `ServerApp.http` deleted.

## Full ordered commit sequence (as planned)

| # | Branch | Message |
|---|--------|---------|
| 1 | `main` | `chore: scaffold ClientApp, ServerApp, and solution (reset from Azure SWA debris)` |
| 2 | branch | `feat(activity1): scaffold FetchProducts component and Product model` |
| 3 | branch | `feat(activity1): implement API integration in OnInitializedAsync` |
| 4 | branch | `feat(activity1): add error handling and refine integration code` |
| — | — | *(Gate 1)* |
| 5 | branch | `fix(activity2): align front-end and back-end API route to /api/productlist` |
| 6 | branch | `fix(activity2): add CORS policy to allow front-end access` |
| 7 | branch | `fix(activity2): add try-catch error handling for JSON deserialization` |
| — | — | *(Gate 2)* |
| 8 | branch | `feat(activity3): add nested category object to JSON response and front-end model` |
| — | — | *(Gate 3 — JSON CONTRACT FREEZE begins)* |
| 9 | branch | `docs(activity4): document AI-assisted integration decisions in code comments` |
| 10 | branch | `perf(activity4): reduce redundant front-end calls and cache backend responses` |
| 11 | branch | `docs(activity4): add project reflection` |
| — | — | *(Gate 4)* |
| 12 | branch | `chore(deploy): add Dockerfile for Render backend hosting` |
| 13 | branch | `docs(deploy): record Azure evaluation and Render decision` |
| 14 | branch | `chore(deploy): wire placeholder Render URL into front-end config` |
| — | — | *(Deployment Gate)* |
| 15 | branch | `fix(deploy): wire live Render backend URL` |
| 16 | branch | `ci: add GitHub Pages workflow for ClientApp` |
| 17 | branch | `docs: add README, project docs, and learning-mode walkthrough` |
| 18 | branch | `docs: archive build plan and completion report` (last commit this run) |

19 commits total as planned (1 direct to `main`, 18 inside the PR).

## Key implementation notes carried into execution

- CORS, in-memory data, and the placeholder URL each get an inline comment or doc line stating
  plainly what they are — never framed as secure or production-ready.
- JSON contract freeze: from the Activity 3 commit onward, `/api/productlist`'s shape does not
  change field names or nesting; Activity 4's perf commit only adds `IMemoryCache`.
- AI attribution: only the exact phrase "AI coding assistant" appears in REFLECTION.md and the
  Activity 4 code comments — never a product/vendor name.
- No cloud-console action: `az` is never invoked, and neither the Render nor Azure dashboard/API
  is reached — the deployment gate stops and waits for the principal to act manually.
- Archive: `handoff/v1/` (no prior convention existed in this repo).

## Verification (task 24, before archiving)

1. `dotnet build` zero errors/zero warnings for both projects on the final branch state.
2. Every internal markdown link resolves, reported N/N.
3. Every commit's author and committer is `jdsaire`; no AI or `on-behalf-of` text anywhere.
4. `/api/productlist`'s shape diffed from its Activity 3 commit against the final state.
5. The PR against `main` is open and unmerged.

---

## Amendment — deployment scope cut (approved mid-build, before further code was written)

All four Activity gates passed with approval. 15 commits landed on `deploy/v1-fullstack-build`
(plus the 1 scaffold commit on `main`), including one deviation commit beyond the plan above —
`fix(activity2): deserialize productlist JSON case-insensitively` — added mid-Activity-2 after
the principal's own browser check caught a silent camelCase/PascalCase deserialization mismatch
that no later Activity would otherwise have fixed. The Dockerfile, `docs/deployment.md`, and the
placeholder→real Render URL wiring landed as planned.

Render deployment then hit a real, reproducible failure: `Dockerfile Path` in Render's own
dashboard is resolved relative to the repository root, not to the service's `Root Directory`
setting — confirmed against Render's own docs (`render.com/docs/docker`,
`render.com/docs/monorepo-support`) mid-session. The corrected value
(`src/ServerApp/Dockerfile`) was given to the principal, but the deploy still failed after
repeated attempts. **The principal decided to abandon the Render deployment and defer it to a
future session**, outside this course's scope, to be continued externally after this PR merges.

This does not affect grading readiness: the 30-point rubric is entirely about the four graded
Activities plus the reflection — it never required a working deployment.

**Confirmed decisions:**
- Revert `wwwroot/appsettings.json`'s `ApiBaseUrl` back to the literal placeholder string, since
  the real Render URL doesn't respond and leaving it wired would misrepresent the repo's actual
  working state.
- Keep the deployment commits already on the branch (Dockerfile, `docs/deployment.md`, the
  URL-wiring commits) as a documented attempt, not reverted — mirroring how the Azure evaluation
  is already retained as documented learning.
- Skip the GitHub Pages workflow entirely — deploying a front-end pointed at a placeholder
  backend adds nothing.

**Revised remaining sequence**: revert-to-placeholder commit, a deployment-outcome doc commit,
the README/docs/learning-mode commit (adjusted — no live-Pages-URL section, grading-criteria
maps only to the four Activities), the verify pass, then this archive commit — last, as before.
