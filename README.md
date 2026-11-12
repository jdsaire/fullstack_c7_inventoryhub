# InventoryHub

InventoryHub is an inventory management front-end for a small business: a Blazor WebAssembly
app (`ClientApp`) that displays product data — name, price, stock, and category — served by a
.NET Minimal API back-end (`ServerApp`). It's the Course 7 (Full-Stack Integration) capstone
project, built as four graded Activities: generating the front-end/back-end integration,
debugging the issues that surfaced, structuring the JSON response, and optimizing performance.

## Live Interactive Demonstration

Explore the live, interactive web application deployed on **GitHub Pages**:
🔗 **[https://jdsaire.github.io/fullstack_c7_inventoryhub/](https://jdsaire.github.io/fullstack_c7_inventoryhub/)**

The demonstration features:
- **[Interactive Product Catalog](https://jdsaire.github.io/fullstack_c7_inventoryhub/products.html)**: Real-time category filtering (Electronics, Accessories, Home Office), dynamic search, and stock status indicators powered by a bundled JSON snapshot badged `[SIM]`.
- **[Automated Test Runner](https://jdsaire.github.io/fullstack_c7_inventoryhub/tests.html)**: Interactive in-browser verification suite executing test assertions across all 22 test fixtures.
- **Accessible Dark Theme**: WCAG 2.1 AAA accessible visual design system modeled after Course 8 SafeVault standards.

## Tech stack

- **Front-end**: Blazor WebAssembly (standalone), .NET 10
- **Back-end**: ASP.NET Core Minimal API, .NET 10, in-memory data, `IMemoryCache`
- **Solution**: `src/FullStackSolution.slnx`

## Documentation

- [docs/](docs/) — setup requirements, grading-criteria mapping, and activity completion reports
- [docs/reports/](docs/reports/) — activity-by-activity completion reports
- [REFLECTION.md](REFLECTION.md) — the reflective summary on using AI assistance across this build

## Out of scope

By design, not by oversight — the course mandate sets Blazor WebAssembly + .NET Minimal API
only, so this project deliberately has: no database (product data is in-memory and does not
survive a restart), no authentication or authorization, and a CORS policy
(`AllowAnyOrigin`/`AllowAnyMethod`/`AllowAnyHeader`) that is course-literal and demo-permissive
— it is not a production security stance, and nothing in this repository claims otherwise.

## AI Assistance Disclosure

Built with the assistance of Google Antigravity for the current rebuild, and an AI coding assistant for initial scaffolding — see [REFLECTION.md](REFLECTION.md) for specifics on where and how.
