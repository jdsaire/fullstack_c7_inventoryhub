using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Registers the CORS services that the UseCors middleware below depends on.
// Without this the app compiles but throws at startup ("Unable to resolve service
// for type 'ICorsService'"), so it is required for the policy to work at all.
builder.Services.AddCors();

// IMemoryCache ships with ASP.NET Core (Microsoft.Extensions.Caching.Memory) — no new
// package reference is needed to use it.
builder.Services.AddMemoryCache();

var app = builder.Build();

// Integration point: CORS policy. An AI coding assistant diagnosed why the policy alone
// produced a startup crash (ICorsService could not be resolved) and identified the missing
// AddCors() registration above as the fix, distinct from the policy content itself.
//
// Course-literal, demo-permissive CORS policy: it accepts any origin, method, and header.
// This is NOT a production security stance and protects against nothing — it exists so the
// Blazor front-end (a different origin during local development) can reach this API at all.
// A real deployment would name the specific allowed origin instead.
app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

// Performance: caches the product list response so repeated requests recompute nothing —
// the data is static in-memory data anyway, but this establishes the pattern for when it's
// backed by real, more expensive work (a database query, an external call). The response
// shape returned here is unchanged from Activity 3 — only the caching path is new.
app.MapGet("/api/productlist", (IMemoryCache cache) =>
{
    return cache.GetOrCreate("productlist", entry =>
    {
        entry.SlidingExpiration = TimeSpan.FromMinutes(5);
        return new[]
        {
            new
            {
                Id = 1,
                Name = "Laptop",
                Price = 1200.50,
                Stock = 25,
                Category = new { Id = 101, Name = "Electronics" }
            },
            new
            {
                Id = 2,
                Name = "Headphones",
                Price = 50.00,
                Stock = 100,
                Category = new { Id = 102, Name = "Accessories" }
            }
        };
    });
});

app.Run();
