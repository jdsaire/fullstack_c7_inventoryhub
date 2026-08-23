# 1. Front-end/back-end integration

Activity 1's job: get Blazor to actually show data from the Minimal API. Three commits, in
order, on `deploy/v1-fullstack-build`.

## Scaffold first, wire it up second

`feat(activity1): scaffold FetchProducts component and Product model` created
[FetchProducts.razor](../src/ClientApp/Pages/FetchProducts.razor) with a `Product` class and an
empty `OnInitializedAsync` — no HTTP call yet. This intentionally builds and runs (showing
"Loading..." forever), which is the checkpoint before adding real integration logic on top of
it.

One fix was needed just to get this far: the mandate's starter markup has a `foreach` loop
missing its closing parenthesis (`foreach (var product in products` with no `)`) — a plain
syntax error, corrected without changing what the loop does.

## The actual integration

`feat(activity1): implement API integration in OnInitializedAsync` filled in the empty method:
`HttpClient.GetAsync("/api/products")`, `EnsureSuccessStatusCode()`,
`ReadAsStringAsync()`, `JsonSerializer.Deserialize<Product[]>()`. The `HttpClient`'s
`BaseAddress` is registered in [Program.cs](../src/ClientApp/Program.cs) — at this stage,
hardcoded to ServerApp's local dev URL directly (before the config-based approach used later for
deployment).

## Error handling, readability

`feat(activity1): add error handling and refine integration code` wrapped the call in a
try-catch (`HttpRequestException`, `TaskCanceledException`) and pulled the logic into a separate
`LoadProductsAsync()` method instead of inlining it in `OnInitializedAsync`.

## Why the gate didn't fully pass on the first try

Running both projects at this point produces a browser error:
`TypeError: Load failed`. That's expected, not a bug in this Activity's code — see
[02-Debugging-CORS-and-JSON.md](02-Debugging-CORS-and-JSON.md) for why, and why it's Activity 2's
job to fix, not this one's.
