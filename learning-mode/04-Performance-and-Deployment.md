# 4. Performance and deployment

Activity 4's graded scope was performance; deployment was additional scope pursued afterward, by
choice, beyond the four Activities. Both are covered here since they happened back to back.

## Documenting the integration points

`docs(activity4): document AI-assisted integration decisions in code comments` added a short
comment at each integration point — the `HttpClient` call, the CORS policy, the JSON error
handling, and the nested-category deserialization — explaining how an AI coding assistant
contributed to that specific piece, using only that phrase (never a product or vendor name).

## The actual optimization

`perf(activity4): reduce redundant front-end calls and cache backend responses`:

- **Front-end**: confirmed the fetch-on-init pattern wasn't duplicated — `OnInitializedAsync`
  runs once per component instance in Blazor's lifecycle, not on every re-render, so there was
  nothing to fix there, only to verify. The three near-identical `catch` blocks (each just
  logging and setting a message) were deduplicated into one `LogAndSetError` helper.
- **Back-end**: `IMemoryCache` added around the `/api/productlist` handler in
  [Program.cs](../src/ServerApp/Program.cs). The response shape is unchanged from Activity 3 —
  confirmed by comparing `curl` output before and after — only the caching path is new.

## The reflection

`docs(activity4): add project reflection` — [REFLECTION.md](../REFLECTION.md), the required
three-point summary: how the AI coding assistant helped, what challenges came up, what was
learned.

## Deployment: attempted, not completed

Beyond the four Activities, Dockerfile and hosting work were pursued: an Azure evaluation that
turned out to need the wrong resource type and then a multi-step identity setup disproportionate
to this project's scope, followed by choosing Render instead. The Render Web Service itself was
created, and a real, specific failure was root-caused: Render's **Dockerfile Path** setting
resolves relative to the repository root, not to the service's separately configured **Root
Directory** — a genuine platform-specific gotcha, not a bug in this repository's Dockerfile. The
corrected path still didn't resolve the deploy after repeated attempts, and the decision was
made to defer completing it to a future session, outside this course's scope.

Full account, including the exact error message and the documentation sources used to diagnose
it: [docs/deployment.md](../docs/deployment.md).
