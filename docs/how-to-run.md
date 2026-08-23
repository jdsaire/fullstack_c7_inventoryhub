# How to run InventoryHub locally

Both projects run independently, in two terminals, matching the course mandate's own Step 1
instructions.

## 1. ServerApp (back-end)

```bash
cd src/ServerApp
dotnet run
```

By default this listens on `http://localhost:5144` (see
[Properties/launchSettings.json](../src/ServerApp/Properties/launchSettings.json)). Confirm it's
working by browsing to `http://localhost:5144/api/productlist` — you should see a JSON array of
products, each with a nested `category` object.

## 2. ClientApp (front-end)

In a second terminal:

```bash
cd src/ClientApp
dotnet run
```

By default this listens on `http://localhost:5048`. Open
`http://localhost:5048/fetchproducts` in a browser — you should see the product list rendered
with live data from ServerApp.

ClientApp's API base URL is read from
[`wwwroot/appsettings.Development.json`](../src/ClientApp/wwwroot/appsettings.Development.json),
which points at ServerApp's local dev URL above — this is separate from
[`wwwroot/appsettings.json`](../src/ClientApp/wwwroot/appsettings.json), which holds the
placeholder Render URL and is not used in local Development runs. See
[deployment.md](deployment.md) for why.

## Building both projects

From `src/`:

```bash
dotnet build FullStackSolution.slnx
```

Should complete with 0 errors and 0 warnings.
