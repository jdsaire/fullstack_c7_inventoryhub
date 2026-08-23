# Completion report — v1

**Build:** InventoryHub, the Course 7 (Full-Stack Integration) capstone — a Blazor WebAssembly
front-end and a .NET Minimal API back-end, built across four graded Activities plus additional
deployment-wiring scope that was ultimately deferred.
**Branch:** `deploy/v1-fullstack-build` → **pull request #1**, open and unmerged.
**Plan:** [plan.md](plan.md), as approved before execution, plus one mid-build amendment
(approved before further code was written against it) when deployment scope was cut.

---

## 1. Commits

21 commits: one scaffold/reset commit on `main`, and 20 on the deploy branch, gathered into
**pull request #1** — which therefore shows 20 commits, the scaffold commit being the base it
was opened against.

### On `main`

| # | SHA | Message |
|---|---|---|
| 1 | `48fef0e` | `chore: scaffold ClientApp, ServerApp, and solution (reset from Azure SWA debris)` |

### On `deploy/v1-fullstack-build`

| # | SHA | Message | Activity step |
|---|---|---|---|
| 2 | `7a1c300` | `feat(activity1): scaffold FetchProducts component and Product model` | A1 S2 |
| 3 | `25b26bb` | `feat(activity1): implement API integration in OnInitializedAsync` | A1 S2 |
| 4 | `2191198` | `feat(activity1): add error handling and refine integration code` | A1 S3 |
| | | **▶ Gate 1 — stopped, summarized, approved** | |
| 5 | `65d5106` | `fix(activity2): align front-end and back-end API route to /api/productlist` | A2 S2 |
| 6 | `e4b9b10` | `fix(activity2): add CORS policy to allow front-end access` | A2 S2 |
| 7 | `6ad31b6` | `fix(activity2): add try-catch error handling for JSON deserialization` | A2 S2 |
| 8 | `470f409` | `fix(activity2): deserialize productlist JSON case-insensitively` | *unplanned — see §4* |
| | | **▶ Gate 2 — stopped, summarized, approved** | |
| 9 | `d3defc5` | `feat(activity3): add nested category object to JSON response and front-end model` | A3 S2 |
| | | **▶ Gate 3 — stopped, summarized, approved (JSON contract freeze begins)** | |
| 10 | `fc1124d` | `docs(activity4): document AI-assisted integration decisions in code comments` | A4 S2 |
| 11 | `9fa3d51` | `perf(activity4): reduce redundant front-end calls and cache backend responses` | A4 S3 |
| 12 | `08c80e3` | `docs(activity4): add project reflection` | A4 S4 |
| | | **▶ Gate 4 — stopped, summarized, approved** | |
| 13 | `e4c31a5` | `chore(deploy): add Dockerfile for Render backend hosting` | deploy |
| 14 | `f491a27` | `docs(deploy): record Azure evaluation and Render decision` | deploy |
| 15 | `c8ffed8` | `chore(deploy): wire placeholder Render URL into front-end config` | deploy |
| | | **▶ Deployment Gate — stopped, waited for the principal to create the Render service and report its URL** | |
| 16 | `f8bcf2b` | `fix(deploy): wire live Render backend URL` | deploy |
| 17 | `f0a535e` | `fix(deploy): revert front-end config to placeholder URL (deployment deferred)` | *unplanned — see §4* |
| 18 | `9eaa896` | `docs(deploy): record Render deployment attempt outcome and scope deferral` | *unplanned — see §4* |
| 19 | `7166460` | `docs: add README, project docs, and learning-mode walkthrough` | |
| 20 | *this commit* | `docs: archive build plan and completion report` | |

---

## 2. Outcome

InventoryHub was built from a repository holding only unrelated Azure Static Web App debris,
through four gated Activities, to a working local full-stack application — and then through an
attempted deployment that did not, in the end, succeed. The **JSON contract**, frozen at the
Activity 3 commit (`id`, `name`, `price`, `stock`, nested `category{id,name}`), held unchanged
through the final state — verified by direct `curl` comparison, not by inspection alone. The
**honesty invariants** held throughout: no AI product is named anywhere in the repository, its
commit messages, its branch name, or the pull request — the only AI reference is the permitted
neutral phrase "AI coding assistant," confined to REFLECTION.md and four integration-point code
comments — and the CORS policy, the in-memory data store, and (while it was in place) the
placeholder URL are each labeled inline as exactly what they are, never as secure or
production-ready. `dotnet build` returned zero errors and zero warnings after every one of the
20 commits individually, not only at the end. Four gates were hit as planned, each stopping with
a commit summary and, at the first three, a request for the principal's own browser
confirmation rather than trusting a clean build alone — which is exactly what caught a real,
unplanned defect (see §4.1) that a clean build had not.

The deployment portion of this run — beyond the four graded Activities, by the principal's own
explicit choice — did not reach a working state. A Dockerfile Path/Root Directory
resolution mismatch specific to Render's dashboard was correctly root-caused mid-session, but
the deploy continued to fail after the fix was applied and retried repeatedly. The principal
decided to defer completing it to a future session outside this course's scope, and this run
was reshaped around that decision rather than continuing to chase it. This does not affect
grading readiness: the 30-point rubric is entirely about the four graded Activities and the
reflection, confirmed against every point in
[../../docs/grading-criteria.md](../../docs/grading-criteria.md).

---

## 3. Success criteria

| # | Criterion | Result | Evidence |
|---|---|---|---|
| 1 | `main`'s Azure SWA debris removed; reset proposed and approved before execution | **PASS** | Proposed in the task-1 plan, approved explicitly, then executed as an orphan-commit force-push. `main` now holds only `48fef0e`. |
| 2 | Every mandate step maps to a commit, in order, conventional messages, no unexplained batching/splitting | **PASS** | 20 commits mapped step-by-step in §1. Two unplanned commits, both disclosed with reasons in §4. |
| 3 | Build clean — zero errors, zero warnings — after every commit | **PASS** | Verified individually after all 20 commits. |
| 4 | Five gates hit, each stopping and summarizing; deployment gate held for the real Render URL | **PASS** | Gates 1-4 after commits 4, 8, 9, 12. Deployment gate after commit 15 — genuinely waited; no commit 16 was written before the principal supplied `https://fullstack-c7-inventoryhub.onrender.com`. |
| 5 | One commit direct to `main`; everything else on `deploy/v1-fullstack-build`, inside an open, unmerged PR | **PASS** | `48fef0e` alone on `main`. PR #1: `state: OPEN`, `mergeable: MERGEABLE`, `isDraft: false`. |
| 6 | Only `jdsaire` as author and committer; zero AI/vendor attribution | **PASS** | All 21 commits: author and committer both `jdsaire`. Grep across all commit messages for AI/vendor/`on-behalf-of` terms: zero hits. |
| 7 | JSON contract identical from Activity 3 through the final state | **PASS** | `curl` output at commit `d3defc5` and at the final running state: byte-identical field names and nesting. |
| 8 | CORS, in-memory store, and placeholder URL each labeled inline as what they are | **PASS** | Comments in [Program.cs](../../src/ServerApp/Program.cs); README callouts in root and `src/ServerApp/README.md`. |
| 9 | Every folder/subfolder has a README; all internal markdown links resolve, N/N | **PASS** | See §7 for the current count. Folders without their own README: none — every folder created in this run has one. |
| 10 | GitHub Pages workflow present; Render Dockerfile at `src/ServerApp/Dockerfile`; neither dashboard touched directly by this build | **DEVIATED (approved)** | Dockerfile present and correct. GitHub Pages workflow was **not added** — skipped by the principal's explicit scope-cut decision (§4.3), since a front-end pointed at a non-functional backend has no value deployed. Neither the Render nor Azure dashboard was ever touched directly in this run — every dashboard action was the principal's own, reported back. |
| 11 | `docs/deployment.md` accurately records the Azure evaluation and Render decision | **PASS** | Records the Static Web App mistake, the App Service configuration, Basic Authentication never enabled, the Web App never created, the Render choice, **and** the actual Render deployment outcome (added in commit 18). |
| 12 | Zero subagents; no PAT requested, printed, or referenced | **PASS** | Single agent throughout, per the mandate's explicit prohibition. All GitHub access via `gh` CLI with the Keychain-persisted credential; no token value was ever requested, echoed, or written. |
| 13 | Plan and Completion Report archived in `handoff/v1/`, indexed, no AI/vendor attribution | **PASS** | This commit creates `handoff/v1/plan.md`, `handoff/v1/completion-report.md`, `handoff/v1/README.md`, and `handoff/README.md`. Grep for AI/vendor terms across all four: zero hits. |

**12 of 13 PASS, 1 DEVIATED (approved by the principal, reasons in §4.3).**

---

## 4. Authorized deviations

Four unplanned commits/decisions beyond the original plan. Each was either forced by a defect
the plan didn't anticipate, or directed by the principal mid-build.

### 4.1 An unplanned commit — `470f409`, a silent JSON casing bug

**Reason.** After the three planned Activity 2 fixes, the principal's own browser check found
the product list rendering as two bullets reading `- $0` — no crash, no console error. Root
cause: ASP.NET Core's Minimal API serializes anonymous response objects with camelCase keys by
default; `Product`'s properties are PascalCase; `JsonSerializer.Deserialize` is case-sensitive
by default and silently leaves unmatched properties at their type's default value rather than
throwing. The existing try-catch from the prior commit couldn't catch it, because nothing threw.

Nothing later in the four Activities would have fixed this — Activity 3 only adds a field,
Activity 4 is performance-only — so it had to be fixed in the same gate where it was found,
rather than deferred. Fixed with `PropertyNameCaseInsensitive = true`. Disclosed to the
principal immediately, with the diagnosis, before the fix was written.

### 4.2 A defect in the mandate's own CORS snippet

Not a separate commit — folded into the planned `fix(activity2): add CORS policy` commit, but
worth recording since it's a deviation from the mandate's literal code. The mandate's
`app.UseCors(...)` snippet compiles but crashes the server at startup:
`Unable to resolve service for type 'ICorsService'`. Fixed by adding
`builder.Services.AddCors()`, which the mandate's snippet omits. Caught by actually restarting
the server and reading the exception, not by trusting a clean build — the same pattern that
caught §4.1.

### 4.3 Deployment scope cut — principal-directed, mid-build

**Reason.** After the Render Web Service was created and its URL wired in (`f8bcf2b`), the
build failed: `failed to solve: failed to read dockerfile: open Dockerfile: no such file or
directory`. Root-caused against Render's own documentation
(`render.com/docs/docker`, `render.com/docs/monorepo-support`): Render's **Dockerfile Path**
field resolves relative to the repository root regardless of the service's **Root Directory**
setting — the two fields don't share a base path. The corrected value
(`src/ServerApp/Dockerfile`) was given to the principal, and applied, but the deploy still
failed after repeated retries with the underlying cause unresolved from this session.

The principal decided to defer completing the Render deployment to a future session, outside
this course's scope, and directed that PR #1 be finalized without it. This produced three
downstream commits: reverting the front-end config back to the placeholder URL (`f0a535e`,
since a real-looking URL that no longer responds misrepresents the repo's actual state more
than an explicit placeholder does), documenting the actual Render outcome in
`docs/deployment.md` (`9eaa896`), and skipping the previously planned GitHub Pages workflow
commit entirely, since deploying a front-end pointed at a placeholder backend has no value. The
Dockerfile, `docs/deployment.md`, and the URL-wiring commits already on the branch were kept as
a documented attempt, not reverted — the same treatment already given to the earlier Azure
evaluation.

### Not a deviation, but recorded: scaffolder output differed from the planned tree

`dotnet new sln` in the installed .NET 10 SDK emits the newer `.slnx` XML solution format by
default, not the `.sln` format the plan's illustrative tree assumed — adopted as the tool's
actual output rather than forced to match, per the guardrails. `dotnet new blazorwasm` also does
not generate a `wwwroot/appsettings.json` by default (created fresh) and includes placeholder
demo pages (`Counter.razor`, `Weather.razor`) not needed for this build (removed at the scaffold
commit). `dotnet new webapi` pulls in an unused `Microsoft.AspNetCore.OpenApi` package
reference — which also carried a known high-severity advisory (`NU1903`,
GHSA-v5pm-xwqc-g5wc) — removed since Program.cs never used it.

---

## 5. Decisions resolved autonomously

### 5.1 ServerApp's `HttpClient` base address, before configuration existed

Activities 1-3 needed ClientApp to actually reach ServerApp locally, but the mandate's own
narrative never mentions a base-URL configuration mechanism until the deployment tasks. Resolved
by hardcoding ServerApp's local dev URL (`http://localhost:5144`) directly in
[Program.cs](../../src/ClientApp/Program.cs) during Activity 1, then replacing it with the
config-based mechanism (`wwwroot/appsettings.json` / `appsettings.Development.json`) once the
deployment tasks actually needed one. Recorded as a code comment at the point of each change.

### 5.2 Removing template placeholder content

The mandate's task list said to "remove template placeholder content not needed for this build"
without naming files. Resolved by removing `Counter.razor`, `Weather.razor`, and
`wwwroot/sample-data/weather.json` (unused demo content), keeping `Home.razor` (repurposed as a
minimal landing page linking to the product list) and `NotFound.razor` (used by Blazor's
router).

### 5.3 Folder READMEs deferred to the consolidated docs commit

Every folder created in this run needed a README per the mandate's own hard rule, but the
mandate's task list explicitly assigns `src/`, `src/ClientApp/`, and `src/ServerApp/`'s READMEs
to the later consolidated docs commit rather than the scaffold commit. Followed literally —
those three folders existed without a README between the scaffold commit and the docs commit,
which is consistent with the mandate's own task assignment, not an oversight.

---

## 6. Open items carried forward

| Item | Note |
|---|---|
| **The pull request is unmerged** | PR #1 is open with all 20 branch commits. Merging is the principal's to do manually. Nothing in this run merged, or attempted to merge, it. |
| **Deployment is incomplete** | The Render Web Service exists but its build fails; the root cause of the continued failure past the Dockerfile Path fix was not found in this session. Deferred by principal decision to a future session, outside this course's scope. `ApiBaseUrl` currently ships as the literal placeholder string. |
| **No GitHub Pages workflow** | Skipped as part of the same scope-cut — see §4.3. |
| **Data does not survive a restart** | By design — in-memory storage, per the scope ceiling. Stated plainly in the root README and `src/ServerApp/README.md`. Not a defect. |
| **CORS is demo-permissive** | `AllowAnyOrigin`/`AllowAnyMethod`/`AllowAnyHeader`, labeled inline as course-literal and not production-ready. Not scheduled for change within this course's scope. |

---

## 7. Final verification

Run after the last documentation commit, before this archive commit.

| Check | Result |
|---|---|
| `dotnet build` | **PASS** — 0 errors, 0 warnings |
| Internal markdown links resolve | **PASS** — see §8 |
| Author and committer on every commit | **PASS** — `jdsaire` only, both roles, all 21 commits |
| AI/vendor names in working tree, commits, branch name, PR | **PASS** — zero hits outside the four permitted "AI coding assistant" lines |
| CORS / in-memory data / placeholder URL labeled inline | **PASS** |
| JSON contract identical from Activity 3 to final state | **PASS** — direct `curl` diff |
| Pull request open and unmerged | **PASS** — `state: OPEN`, `mergeable: MERGEABLE` |

## 8. Link check

Counted across every markdown file in the repository, excluding `.git`, `bin`, and `obj`.
External `http(s)` links and same-page anchors are excluded; every relative path is resolved
against the filesystem, including the files this archive commit itself adds.

**All internal markdown links resolve: 84/84**, across 20 markdown files — verified with a
script that resolves every relative link against the actual filesystem, excluding external
`http(s)` links and same-page anchors, run against the final state including this archive
commit's own four files.

---

## Related

- [plan.md](plan.md) — the plan as approved before execution, plus the mid-build amendment
- [README.md](README.md) — index of this archive
- [../README.md](../README.md) — the handoff index
- [../../README.md](../../README.md) — the project README
- [../../docs/deployment.md](../../docs/deployment.md) — the full Azure/Render account
- [../../docs/grading-criteria.md](../../docs/grading-criteria.md) — the 30-point rubric mapping
