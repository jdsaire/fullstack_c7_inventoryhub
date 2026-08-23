# Grading criteria mapping

The 30-point rubric from the course mandate, mapped to the actual commits and files that
satisfy each point. All commits below are on `deploy/v1-fullstack-build`, in PR #1 against
`main`, except the scaffold commit, which is `main`'s single commit.

## (5 pts) Did you create a GitHub repository for your project?

`jdsaire/fullstack_c7_inventoryhub`, public, with the scaffold as its history's first commit
(`chore: scaffold ClientApp, ServerApp, and solution (reset from Azure SWA debris)`).

## (5 pts) Did you generate and refine integration code for front-end and back-end communication?

Activity 1, three commits:
- `feat(activity1): scaffold FetchProducts component and Product model` —
  [src/ClientApp/Pages/FetchProducts.razor](../src/ClientApp/Pages/FetchProducts.razor)
- `feat(activity1): implement API integration in OnInitializedAsync`
- `feat(activity1): add error handling and refine integration code`

## (5 pts) Did you debug and resolve integration issues effectively using an AI coding assistant?

Activity 2, four commits (one more than the mandate's three — see below):
- `fix(activity2): align front-end and back-end API route to /api/productlist` —
  [src/ServerApp/Program.cs](../src/ServerApp/Program.cs)
- `fix(activity2): add CORS policy to allow front-end access`
- `fix(activity2): add try-catch error handling for JSON deserialization`
- `fix(activity2): deserialize productlist JSON case-insensitively` — a real bug the mandate's
  own scenario doesn't name: ServerApp's default camelCase JSON keys didn't match ClientApp's
  PascalCase `Product` properties, so every field silently bound to its default value with no
  exception thrown. Caught during manual verification, fixed with
  `PropertyNameCaseInsensitive = true`; see [REFLECTION.md](../REFLECTION.md) for the full story.

## (5 pts) Did you create and implement JSON structures for API communication?

Activity 3, one commit: `feat(activity3): add nested category object to JSON response and
front-end model` — the nested `Category` object in both
[src/ServerApp/Program.cs](../src/ServerApp/Program.cs) and
[src/ClientApp/Pages/FetchProducts.razor](../src/ClientApp/Pages/FetchProducts.razor). Verified
directly against a live `curl` of `/api/productlist`.

## (5 pts) Did you optimize the integration code for performance using an AI coding assistant?

Activity 4, one commit: `perf(activity4): reduce redundant front-end calls and cache backend
responses` — `IMemoryCache` added to `/api/productlist` in ServerApp, and the front-end's three
near-identical `catch` blocks deduplicated into one `LogAndSetError` helper. Confirmed the
front-end's fetch-on-init pattern was already single-call (Blazor's `OnInitializedAsync` runs
once per component instance) — nothing there needed changing, only confirming.

## (5 pts) Did you include a reflective summary explaining how the AI coding assistant assisted in each step?

[REFLECTION.md](../REFLECTION.md), commit `docs(activity4): add project reflection` — covers
integration, debugging, JSON structuring, and optimization, plus challenges and what was
learned, using only the phrase "AI coding assistant."

---

Deployment work (Dockerfile, `docs/deployment.md`, the Render attempt) is additional scope the
project owner chose to pursue beyond these six criteria — see
[deployment.md](deployment.md) for why it isn't part of this submission's live state. It does
not affect any of the points above.
