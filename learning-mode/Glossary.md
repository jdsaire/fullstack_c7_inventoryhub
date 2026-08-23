# Glossary

Terms used across the [learning-mode/](README.md) walkthrough.

**Blazor WebAssembly (WASM)** — a .NET front-end framework that runs entirely in the browser via
WebAssembly, rather than on a server. ClientApp is a "standalone" Blazor WASM app: it has no
server-side component of its own, only static files served to the browser.

**Minimal API** — ASP.NET Core's lightweight style for defining HTTP endpoints (`app.MapGet(...)`
etc.) without the full MVC controller/routing scaffolding. ServerApp is built entirely this way.

**CORS (Cross-Origin Resource Sharing)** — the browser security mechanism that blocks a web page
from reading responses from a different origin (different host, port, or scheme) unless that
origin explicitly allows it via response headers. ClientApp (`:5048`) and ServerApp (`:5144`)
are different origins during local development, which is why CORS applies at all.

**camelCase vs. PascalCase** — two naming conventions for identifiers: `productName` (camelCase,
first word lowercase) vs. `ProductName` (PascalCase, every word capitalized). ASP.NET Core
serializes JSON with camelCase keys by default; C# class properties are conventionally
PascalCase. The mismatch between the two is what caused the silent deserialization bug in
[02-Debugging-CORS-and-JSON.md](02-Debugging-CORS-and-JSON.md).

**`IMemoryCache`** — an in-process caching abstraction built into ASP.NET Core
(`Microsoft.Extensions.Caching.Memory`). Used in Activity 4 to cache `/api/productlist`'s
response without adding any new package dependency.

**JSON contract** — the agreed shape of a JSON response (field names, types, nesting) that
consumers depend on. "Freezing" a contract means committing not to change that shape, even while
other things about the endpoint (performance, caching) continue to change.

**Cold start** — when a hosting platform's free tier spins a sleeping service back up in
response to the first request after a period of inactivity, adding latency to just that first
request. Referenced in [docs/deployment.md](../docs/deployment.md) as an expected Render
free-tier behavior, separate from the actual deploy failure that was hit.
