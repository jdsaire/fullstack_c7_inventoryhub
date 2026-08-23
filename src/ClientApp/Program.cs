using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ClientApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ApiBaseUrl comes from wwwroot/appsettings.json, which Blazor WASM loads automatically,
// overridden in Development by wwwroot/appsettings.Development.json (ServerApp's local dev
// URL). The base appsettings.json value is a literal placeholder awaiting the real Render
// URL from the deployment gate — see docs/deployment.md.
var apiBaseUrl = builder.Configuration["ApiBaseUrl"]
    ?? throw new InvalidOperationException("ApiBaseUrl is not configured.");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

await builder.Build().RunAsync();
