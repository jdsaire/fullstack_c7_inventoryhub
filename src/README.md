# src/

The two-project solution: [FullStackSolution.slnx](FullStackSolution.slnx) (the SDK's current
default solution format — `.slnx`, not the older `.sln`), and the projects it references.

- [ClientApp/](ClientApp/) — the Blazor WebAssembly front-end
- [ServerApp/](ServerApp/) — the .NET Minimal API back-end

They're independent `dotnet run` processes — see [../docs/how-to-run.md](../docs/how-to-run.md)
for running both locally, or the solution as a whole with `dotnet build FullStackSolution.slnx`.

Back to the [project root](../README.md).
