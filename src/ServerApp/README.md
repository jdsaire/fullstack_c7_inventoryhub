# ServerApp

The .NET Minimal API back-end (`dotnet new webapi`). One endpoint,
[`/api/productlist`](Program.cs), returning in-memory product data — it does not persist a
restart, and there is no database. CORS is configured to allow any origin/method/header, which
is course-literal and demo-permissive, not a production security stance (labeled inline in
[Program.cs](Program.cs)). The response is cached with `IMemoryCache`.

[Dockerfile](Dockerfile) is a multi-stage build intended for Render — see
[../../docs/deployment.md](../../docs/deployment.md) for its current (undeployed) status.

Run it with `dotnet run` from this directory — see
[../../docs/how-to-run.md](../../docs/how-to-run.md) for the full two-terminal setup alongside
ClientApp.

Back to [src/](../README.md).
