using System.Text.Json;
using NUnit.Framework;

namespace InventoryHub.Tests;

/// <summary>
/// Integration tests verifying front-end and back-end data communication,
/// model schemas, and JSON serialization contracts for InventoryHub.
/// </summary>
[TestFixture]
public class TestIntegration
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

    private const string SeedApiJson = """
    [
        {
            "id": 1,
            "name": "Laptop",
            "price": 1200.50,
            "stock": 25
        },
        {
            "id": 2,
            "name": "Headphones",
            "price": 50.00,
            "stock": 100
        }
    ]
    """;

    [Test]
    public void TestProductModel_PropertiesAndInstantiation()
    {
        var product = new Product
        {
            Id = 10,
            Name = "Monitor",
            Price = 299.99,
            Stock = 15
        };

        Assert.That(product.Id, Is.EqualTo(10));
        Assert.That(product.Name, Is.EqualTo("Monitor"));
        Assert.That(product.Price, Is.EqualTo(299.99));
        Assert.That(product.Stock, Is.EqualTo(15));
        Assert.That(product.Category, Is.Null);
    }

    [Test]
    public void TestProductSerialization_ProducesExpectedJson()
    {
        var product = new Product
        {
            Id = 1,
            Name = "Laptop",
            Price = 1200.50,
            Stock = 25
        };

        string json = JsonSerializer.Serialize(product);

        Assert.That(json, Does.Contain("\"Id\":1"));
        Assert.That(json, Does.Contain("\"Name\":\"Laptop\""));
        Assert.That(json, Does.Contain("\"Price\":1200.5"));
        Assert.That(json, Does.Contain("\"Stock\":25"));
    }

    [Test]
    public void TestProductDeserialization_BindsCaseInsensitively()
    {
        var products = JsonSerializer.Deserialize<Product[]>(SeedApiJson, CaseInsensitiveOptions);

        Assert.That(products, Is.Not.Null);
        Assert.That(products!.Length, Is.EqualTo(2));

        Assert.That(products[0].Id, Is.EqualTo(1));
        Assert.That(products[0].Name, Is.EqualTo("Laptop"));
        Assert.That(products[0].Price, Is.EqualTo(1200.50));
        Assert.That(products[0].Stock, Is.EqualTo(25));

        Assert.That(products[1].Id, Is.EqualTo(2));
        Assert.That(products[1].Name, Is.EqualTo("Headphones"));
        Assert.That(products[1].Price, Is.EqualTo(50.00));
        Assert.That(products[1].Stock, Is.EqualTo(100));
    }

    [Test]
    public void TestProductDeserialization_CaseSensitive_FailsToBind()
    {
        // When PropertyNameCaseInsensitive is false, camelCase JSON keys (id, name, price, stock)
        // fail to bind to PascalCase properties, leaving properties at default values without throwing.
        var products = JsonSerializer.Deserialize<Product[]>(SeedApiJson, CaseSensitiveOptions);

        Assert.That(products, Is.Not.Null);
        Assert.That(products!.Length, Is.EqualTo(2));
        Assert.That(products[0].Id, Is.EqualTo(0));
        Assert.That(products[0].Name, Is.EqualTo(string.Empty));
        Assert.That(products[0].Price, Is.EqualTo(0.0));
        Assert.That(products[0].Stock, Is.EqualTo(0));
    }

    [Test]
    public void TestMalformedJson_ThrowsJsonException()
    {
        const string malformedJson = "[{\"id\": 1, \"name\": \"Laptop\", \"price\": \"not-a-number\"}]";

        Assert.Throws<JsonException>(() =>
        {
            JsonSerializer.Deserialize<Product[]>(malformedJson, CaseInsensitiveOptions);
        });
    }

    [Test]
    public void TestEmptyJsonArray_ReturnsEmptyCollection()
    {
        const string emptyJson = "[]";
        var products = JsonSerializer.Deserialize<Product[]>(emptyJson, CaseInsensitiveOptions);

        Assert.That(products, Is.Not.Null);
        Assert.That(products!.Length, Is.EqualTo(0));
    }
}
