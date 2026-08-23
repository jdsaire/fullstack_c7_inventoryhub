# ClientApp

The Blazor WebAssembly front-end — a standalone app (`dotnet new blazorwasm`) with no server
component of its own. The product list lives at
[Pages/FetchProducts.razor](Pages/FetchProducts.razor), which calls ServerApp's
`/api/productlist` endpoint and renders the result.

The API base URL is read from configuration:
[wwwroot/appsettings.json](wwwroot/appsettings.json) (the shipped default — currently a
placeholder, see [../../docs/deployment.md](../../docs/deployment.md)), overridden locally by
[wwwroot/appsettings.Development.json](wwwroot/appsettings.Development.json) when running via
`dotnet run`.

Run it with `dotnet run` from this directory — see
[../../docs/how-to-run.md](../../docs/how-to-run.md) for the full two-terminal setup alongside
ServerApp.

Back to [src/](../README.md).
