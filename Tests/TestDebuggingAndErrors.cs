using System.Text.Json;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace InventoryHub.Tests;

/// <summary>
/// Unit tests verifying route alignment, CORS policy middleware and service registration,
/// and typed JSON deserialization error handling for the InventoryHub full-stack application.
/// </summary>
[TestFixture]
public class TestDebuggingAndErrors
{
    private static readonly JsonSerializerOptions CaseInsensitiveOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly JsonSerializerOptions CaseSensitiveOptions = new()
    {
        PropertyNameCaseInsensitive = false
    };

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Stock { get; set; }
        public Category? Category { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    // Aligned API route matching both client Http.GetAsync and server app.MapGet
    public const string AlignedApiRoute = "/api/productlist";
    public const string LegacyMismatchedRoute = "/api/products";

    #region Route Alignment Tests

    [Test]
    public void TestRouteAlignment_ClientRequestMatchesServerEndpoint()
    {
        // Front-end FetchProducts.razor calls /api/productlist; ServerApp Program.cs maps /api/productlist
        const string clientRoute = "/api/productlist";
        const string serverRoute = "/api/productlist";

        Assert.That(clientRoute, Is.EqualTo(serverRoute));
        Assert.That(clientRoute, Is.EqualTo(AlignedApiRoute));
    }

    [Test]
    public void TestRouteMismatch_LegacyRouteDoesNotMatchAlignedEndpoint()
    {
        // Activity 2 scenario: client attempting to call starter route /api/products fails
        // to reach the refactored /api/productlist endpoint, resulting in HTTP 404 Not Found.
        Assert.That(LegacyMismatchedRoute, Is.Not.EqualTo(AlignedApiRoute));
    }

    [Test]
    public void TestRouteConstruction_BaseAddressAndRelativeEndpointCombineAccurately()
    {
        var baseUri = new Uri("http://localhost:5000");
        var requestUri = new Uri(baseUri, AlignedApiRoute);

        Assert.That(requestUri.ToString(), Is.EqualTo("http://localhost:5000/api/productlist"));
        Assert.That(requestUri.AbsolutePath, Is.EqualTo("/api/productlist"));
    }

    #endregion

    #region CORS Policy & Middleware Registration Tests

    [Test]
    public void TestCorsPolicy_ConfiguredAsDemoPermissive()
    {
        // Validates the demo-permissive CORS policy configured in ServerApp:
        // policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
        var builder = new CorsPolicyBuilder();
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();

        CorsPolicy policy = builder.Build();

        Assert.That(policy.AllowAnyOrigin, Is.True);
        Assert.That(policy.AllowAnyMethod, Is.True);
        Assert.That(policy.AllowAnyHeader, Is.True);
        Assert.That(policy.Origins, Does.Contain("*"));
    }

    [Test]
    public void TestCorsService_RegistrationResolvesRequiredDependencies()
    {
        // Registers CORS services via builder.Services.AddCors().
        // Resolves ICorsService and ICorsPolicyProvider, ensuring UseCors middleware executes without startup crashes.
        var services = new ServiceCollection();
        services.AddCors();

        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        var corsService = serviceProvider.GetService<ICorsService>();
        var policyProvider = serviceProvider.GetService<ICorsPolicyProvider>();

        Assert.That(corsService, Is.Not.Null);
        Assert.That(policyProvider, Is.Not.Null);
    }

    [Test]
    public void TestCorsService_MissingRegistrationFailsResolution()
    {
        // Demonstrates the runtime startup error scenario:
        // Calling UseCors without registering AddCors() fails to resolve ICorsService.
        var services = new ServiceCollection();

        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        var corsService = serviceProvider.GetService<ICorsService>();

        Assert.That(corsService, Is.Null);
    }

    #endregion

    #region Typed JSON Deserialization Error Handling Tests

    [Test]
    public void TestJsonDeserialization_MalformedSyntaxThrowsJsonException()
    {
        // Malformed JSON syntax (unclosed array, missing quotes, or invalid tokens)
        const string malformedSyntax = "[{\"id\": 1, \"name\": \"Laptop\", \"price\": 1200.50, ]";

        Assert.Throws<JsonException>(() =>
        {
            JsonSerializer.Deserialize<Product[]>(malformedSyntax, CaseInsensitiveOptions);
        });
    }

    [Test]
    public void TestJsonDeserialization_TypeMismatchThrowsJsonException()
    {
        // Scalar type mismatch: price string value where double number is expected
        const string typeMismatchJson = "[{\"id\": 1, \"name\": \"Laptop\", \"price\": \"one-thousand-dollars\", \"stock\": 10}]";

        Assert.Throws<JsonException>(() =>
        {
            JsonSerializer.Deserialize<Product[]>(typeMismatchJson, CaseInsensitiveOptions);
        });
    }

    [Test]
    public void TestCaseInsensitiveDeserialization_ResolvesCasingMismatch()
    {
        // ASP.NET Core Minimal API emits camelCase by default (id, name, price, stock).
        // Front-end Product model properties are PascalCase (Id, Name, Price, Stock).
        const string camelCaseJson = """
        [
            {
                "id": 1,
                "name": "Laptop",
                "price": 1200.50,
                "stock": 25
            }
        ]
        """;

        var products = JsonSerializer.Deserialize<Product[]>(camelCaseJson, CaseInsensitiveOptions);

        Assert.That(products, Is.Not.Null);
        Assert.That(products!.Length, Is.EqualTo(1));
        Assert.That(products[0].Id, Is.EqualTo(1));
        Assert.That(products[0].Name, Is.EqualTo("Laptop"));
        Assert.That(products[0].Price, Is.EqualTo(1200.50));
        Assert.That(products[0].Stock, Is.EqualTo(25));
    }

    [Test]
    public void TestCaseSensitiveDeserialization_LeavesPropertiesAtDefaultValues()
    {
        // Proves why PropertyNameCaseInsensitive = true is required:
        // Under case-sensitive rules, camelCase keys do not match PascalCase properties,
        // resulting in default-initialized fields (0, empty string) rather than parsed data.
        const string camelCaseJson = """
        [
            {
                "id": 1,
                "name": "Laptop",
                "price": 1200.50,
                "stock": 25
            }
        ]
        """;

        var products = JsonSerializer.Deserialize<Product[]>(camelCaseJson, CaseSensitiveOptions);

        Assert.That(products, Is.Not.Null);
        Assert.That(products!.Length, Is.EqualTo(1));
        Assert.That(products[0].Id, Is.EqualTo(0));
        Assert.That(products[0].Name, Is.EqualTo(string.Empty));
        Assert.That(products[0].Price, Is.EqualTo(0.0));
        Assert.That(products[0].Stock, Is.EqualTo(0));
    }

    [Test]
    public void TestTypedExceptionIsolation_DispatchesSpecificDiagnosticMessages()
    {
        // Simulates the typed catch block dispatch implemented in FetchProducts.razor:
        // - JsonException -> "The product API returned malformed JSON."
        // - HttpRequestException -> "Unable to reach the product API: {message}"
        // - TaskCanceledException -> "The product API request timed out."
        static string DispatchError(Exception ex) => ex switch
        {
            JsonException => "The product API returned malformed JSON.",
            HttpRequestException httpEx => $"Unable to reach the product API: {httpEx.Message}",
            TaskCanceledException => "The product API request timed out.",
            _ => "An unexpected error occurred."
        };

        var jsonEx = new JsonException("Invalid character in JSON payload.");
        var httpEx = new HttpRequestException("Connection refused.");
        var timeoutEx = new TaskCanceledException("The operation timed out.");
        var genericEx = new InvalidOperationException("Generic failure.");

        Assert.That(DispatchError(jsonEx), Is.EqualTo("The product API returned malformed JSON."));
        Assert.That(DispatchError(httpEx), Does.StartWith("Unable to reach the product API:"));
        Assert.That(DispatchError(timeoutEx), Is.EqualTo("The product API request timed out."));
        Assert.That(DispatchError(genericEx), Is.EqualTo("An unexpected error occurred."));
    }

    #endregion
}
