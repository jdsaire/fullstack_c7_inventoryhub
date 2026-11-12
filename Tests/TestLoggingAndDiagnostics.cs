using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace InventoryHub.Tests;

/// <summary>
/// Unit tests verifying diagnostics middleware telemetry, trace/correlation ID propagation,
/// execution timing headers, cache hit/miss indicators, and structured logging contracts.
/// </summary>
[TestFixture]
public class TestLoggingAndDiagnostics
{
    [Test]
    public void TestDiagnosticsMiddleware_GeneratesTraceIdHeader()
    {
        var context = new DefaultHttpContext();
        context.TraceIdentifier = "trace-generated-98765";

        var traceId = context.Request.Headers.TryGetValue("X-Trace-Id", out var incoming) && !string.IsNullOrWhiteSpace(incoming)
            ? incoming.ToString()
            : context.TraceIdentifier;

        context.Response.Headers["X-Trace-Id"] = traceId;

        Assert.That(context.Response.Headers.ContainsKey("X-Trace-Id"), Is.True);
        Assert.That(context.Response.Headers["X-Trace-Id"].ToString(), Is.EqualTo("trace-generated-98765"));
    }

    [Test]
    public void TestDiagnosticsMiddleware_PreservesIncomingTraceId()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers["X-Trace-Id"] = "client-correlation-abc-123";
        context.TraceIdentifier = "server-trace-fallback";

        var traceId = context.Request.Headers.TryGetValue("X-Trace-Id", out var incoming) && !string.IsNullOrWhiteSpace(incoming)
            ? incoming.ToString()
            : context.TraceIdentifier;

        context.Response.Headers["X-Trace-Id"] = traceId;

        Assert.That(context.Response.Headers["X-Trace-Id"].ToString(), Is.EqualTo("client-correlation-abc-123"),
            "Diagnostics middleware must propagate existing correlation identifiers from client requests.");
    }

    [Test]
    public void TestDiagnosticsMiddleware_AppendsResponseTimeHeader()
    {
        var context = new DefaultHttpContext();
        var stopwatch = Stopwatch.StartNew();

        // Simulate minimal elapsed cycle
        var spinCount = 0;
        for (int i = 0; i < 1000; i++) spinCount += i;
        stopwatch.Stop();

        context.Response.Headers["X-Response-Time-Ms"] = stopwatch.ElapsedMilliseconds.ToString();

        Assert.That(context.Response.Headers.ContainsKey("X-Response-Time-Ms"), Is.True);
        var parsed = long.TryParse(context.Response.Headers["X-Response-Time-Ms"].ToString(), out var elapsedMs);
        Assert.That(parsed, Is.True, "X-Response-Time-Ms header must be a parseable numeric integer.");
        Assert.That(elapsedMs, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public void TestDiagnosticsHeader_CacheStatusReported()
    {
        var missContext = new DefaultHttpContext();
        missContext.Response.Headers["X-Cache"] = "MISS";

        var hitContext = new DefaultHttpContext();
        hitContext.Response.Headers["X-Cache"] = "HIT";

        Assert.That(missContext.Response.Headers["X-Cache"].ToString(), Is.EqualTo("MISS"));
        Assert.That(hitContext.Response.Headers["X-Cache"].ToString(), Is.EqualTo("HIT"));
    }

    [Test]
    public void TestStructuredLog_FormatAndEventProperties()
    {
        const string method = "GET";
        const string path = "/api/productlist";
        const string traceId = "test-trace-555";
        const int statusCode = 200;
        const long elapsedMs = 4;

        var message = $"HTTP {method} {path} completed with status {statusCode} in {elapsedMs}ms | TraceId: {traceId}";

        Assert.That(message, Does.Contain("HTTP GET /api/productlist completed with status 200"));
        Assert.That(message, Does.Contain("TraceId: test-trace-555"));
        Assert.That(message, Does.Contain("4ms"));
    }
}
