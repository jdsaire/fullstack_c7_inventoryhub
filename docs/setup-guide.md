# Setup guide

## Requirements

- **.NET SDK 10** — confirm with `dotnet --version`. `dotnet new list` should include
  `blazorwasm` (Blazor WebAssembly Standalone App) and `webapi` (ASP.NET Core Web API) templates.
- Any editor with C#/Razor support (this project was built with VS Code, per the course
  mandate's own tooling recommendation, but nothing here depends on a specific editor).
- A terminal that can run two `dotnet run` processes side by side.

## First-run walkthrough

1. Clone the repository and open it in your editor.
2. Follow [how-to-run.md](how-to-run.md) to start ServerApp, then ClientApp, in two terminals.
3. Confirm `http://localhost:5144/api/productlist` returns JSON directly.
4. Confirm `http://localhost:5048/fetchproducts` renders the product list in a browser, reading
   from that same API.
5. Run `dotnet build FullStackSolution.slnx` from `src/` to confirm a clean build (0 errors, 0
   warnings) independent of the running dev servers.

No database, no environment secrets, and no external service account are required to run this
project locally — see the root [README.md](../README.md)'s "Out of scope" section for what's
deliberately not included.
