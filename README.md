# InventoryHub

InventoryHub is an inventory management front-end for a small business: a Blazor WebAssembly
app (`ClientApp`) that displays product data — name, price, stock, and category — served by a
.NET Minimal API back-end (`ServerApp`). It's the Course 7 (Full-Stack Integration) capstone
project, built as four graded Activities: generating the front-end/back-end integration,
debugging the issues that surfaced, structuring the JSON response, and optimizing performance.

## How to see it

There's no live deployment yet — see [docs/deployment.md](docs/deployment.md) for exactly why
(an Azure evaluation that didn't fit the project's needs, then a Render attempt that hit an
unresolved dashboard-configuration issue and was deferred). Run it locally instead:
[docs/how-to-run.md](docs/how-to-run.md).

## Tech stack

- **Front-end**: Blazor WebAssembly (standalone), .NET 10
- **Back-end**: ASP.NET Core Minimal API, .NET 10, in-memory data, `IMemoryCache`
- **Solution**: `src/FullStackSolution.slnx`

## Documentation

- [docs/](docs/) — how to run it, setup requirements, the grading-criteria mapping, and the
  deployment history
- [learning-mode/](learning-mode/) — a walkthrough of how the project was actually built, stage
  by stage
- [REFLECTION.md](REFLECTION.md) — the required reflective summary on using an AI coding
  assistant across this build

## Out of scope

By design, not by oversight — the course mandate sets Blazor WebAssembly + .NET Minimal API
only, so this project deliberately has: no database (product data is in-memory and does not
survive a restart), no authentication or authorization, and a CORS policy
(`AllowAnyOrigin`/`AllowAnyMethod`/`AllowAnyHeader`) that is course-literal and demo-permissive
— it is not a production security stance, and nothing in this repository claims otherwise.

## Attribution

Built with the assistance of an AI coding assistant across all four Activities — see
[REFLECTION.md](REFLECTION.md) for specifics on where and how.
