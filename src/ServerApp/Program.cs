using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Registers CORS services required for cross-origin communication between the Blazor
// front-end and this API back-end during development.
builder.Services.AddCors();

// In-memory caching services for high-throughput endpoint response caching.
builder.Services.AddMemoryCache();

// Configures standard console and debug logging.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Diagnostics & Telemetry Middleware:
// Computes request duration, generates/propagates correlation trace IDs, and records
// structured request/response telemetry.
app.Use(async (context, next) =>
{
    var stopwatch = Stopwatch.StartNew();
    
    // Trace/correlation identifier resolution: use incoming header or runtime trace identifier
    var traceId = context.Request.Headers.TryGetValue("X-Trace-Id", out var incomingTraceId) && !string.IsNullOrWhiteSpace(incomingTraceId)
        ? incomingTraceId.ToString()
        : context.TraceIdentifier;

    context.Response.OnStarting(() =>
    {
        stopwatch.Stop();
        context.Response.Headers["X-Trace-Id"] = traceId;
        context.Response.Headers["X-Response-Time-Ms"] = stopwatch.ElapsedMilliseconds.ToString();
        return Task.CompletedTask;
    });

    app.Logger.LogInformation(
        "HTTP {Method} {Path} received | TraceId: {TraceId}",
        context.Request.Method,
        context.Request.Path,
        traceId);

    await next();

    app.Logger.LogInformation(
        "HTTP {Method} {Path} completed with status {StatusCode} in {ElapsedMs}ms | TraceId: {TraceId}",
        context.Request.Method,
        context.Request.Path,
        context.Response.StatusCode,
        stopwatch.ElapsedMilliseconds,
        traceId);
});

// Demo-permissive CORS policy allowing any origin, method, and header.
// Configured specifically for local development and demonstration scenarios.
app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

// Product catalog endpoint:
// Optimized with in-memory caching (5-minute sliding expiration), diagnostic cache status
// headers (X-Cache: HIT / MISS), and structured operational logging.
app.MapGet("/api/productlist", (IMemoryCache cache, HttpContext context, ILoggerFactory loggerFactory) =>
{
    var logger = loggerFactory.CreateLogger("InventoryHub.ProductApi");
    var traceId = context.Response.Headers.TryGetValue("X-Trace-Id", out var tid)
        ? tid.ToString()
        : context.TraceIdentifier;

    if (cache.TryGetValue("productlist", out object? cachedProducts) && cachedProducts is not null)
    {
        context.Response.Headers["X-Cache"] = "HIT";
        logger.LogInformation("Cache HIT for productlist | TraceId: {TraceId}", traceId);
        return Results.Ok(cachedProducts);
    }

    context.Response.Headers["X-Cache"] = "MISS";
    logger.LogInformation("Cache MISS for productlist; populating cache | TraceId: {TraceId}", traceId);

    var products = new[]
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

    var cacheEntryOptions = new MemoryCacheEntryOptions
    {
        SlidingExpiration = TimeSpan.FromMinutes(5)
    };
    cache.Set("productlist", products, cacheEntryOptions);

    return Results.Ok(products);
});

app.Run();
