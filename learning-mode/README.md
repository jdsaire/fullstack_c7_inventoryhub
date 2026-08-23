# learning-mode/

A stage-by-stage walkthrough of how InventoryHub was actually built, for anyone learning the
same material — as opposed to [docs/](../docs/), which describes the project's current state
rather than how it got there.

1. [01-Front-End-Back-End-Integration.md](01-Front-End-Back-End-Integration.md) — Activity 1:
   wiring `HttpClient` calls from Blazor to the Minimal API
2. [02-Debugging-CORS-and-JSON.md](02-Debugging-CORS-and-JSON.md) — Activity 2: the route
   mismatch, the CORS crash, and the JSON casing bug
3. [03-Structuring-JSON-Responses.md](03-Structuring-JSON-Responses.md) — Activity 3: nesting a
   `Category` object in the API response
4. [04-Performance-and-Deployment.md](04-Performance-and-Deployment.md) — Activity 4:
   `IMemoryCache` and refactoring, plus what happened when deployment was attempted

[Glossary.md](Glossary.md) defines terms used across all four.

Back to the [project root](../README.md).
