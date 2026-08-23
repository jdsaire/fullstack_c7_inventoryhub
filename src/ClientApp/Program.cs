using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ClientApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ServerApp's local dev URL (see src/ServerApp/Properties/launchSettings.json).
// Replaced by a configurable, deployment-aware base URL in the deployment-wiring tasks.
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5144") });

await builder.Build().RunAsync();
