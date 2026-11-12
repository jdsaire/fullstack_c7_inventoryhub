using System.Text.Json;
using NUnit.Framework;

namespace InventoryHub.Tests;

/// <summary>
/// Unit tests verifying standardized JSON response structures, nested Category models,
/// nullable safety, and contract serialization for the InventoryHub API.
/// </summary>
[TestFixture]
public class TestJsonStructures
{
    private static readonly JsonSerializerOptions CaseInsensitiveOptions = new()
    {
        PropertyNameCaseInsensitive = true
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

    private const string StandardMultiProductJson = """
    [
        {
            "id": 1,
            "name": "Laptop",
            "price": 1200.50,
            "stock": 25,
            "category": {
                "id": 101,
                "name": "Electronics"
            }
        },
        {
            "id": 2,
            "name": "Headphones",
            "price": 50.00,
            "stock": 100,
            "category": {
                "id": 102,
                "name": "Accessories"
            }
        }
    ]
    """;

    [Test]
    public void TestJsonSchema_ContainsAllStandardProperties()
    {
        var products = JsonSerializer.Deserialize<Product[]>(StandardMultiProductJson, CaseInsensitiveOptions);

        Assert.That(products, Is.Not.Null);
        Assert.That(products!.Length, Is.EqualTo(2));

        Product laptop = products[0];
        Assert.That(laptop.Id, Is.EqualTo(1));
        Assert.That(laptop.Name, Is.EqualTo("Laptop"));
        Assert.That(laptop.Price, Is.EqualTo(1200.50));
        Assert.That(laptop.Stock, Is.EqualTo(25));
        Assert.That(laptop.Category, Is.Not.Null);
    }

    [Test]
    public void TestNestedCategoryModel_DeserializesAccurately()
    {
        var products = JsonSerializer.Deserialize<Product[]>(StandardMultiProductJson, CaseInsensitiveOptions);

        Assert.That(products, Is.Not.Null);
        Product laptop = products![0];

        Assert.That(laptop.Category, Is.Not.Null);
        Assert.That(laptop.Category!.Id, Is.EqualTo(101));
        Assert.That(laptop.Category.Name, Is.EqualTo("Electronics"));
    }

    [Test]
    public void TestMultipleProducts_DistinctNestedCategories()
    {
        var products = JsonSerializer.Deserialize<Product[]>(StandardMultiProductJson, CaseInsensitiveOptions);

        Assert.That(products, Is.Not.Null);
        Assert.That(products!.Length, Is.EqualTo(2));

        Category? cat1 = products[0].Category;
        Category? cat2 = products[1].Category;

        Assert.That(cat1, Is.Not.Null);
        Assert.That(cat2, Is.Not.Null);
        Assert.That(cat1!.Name, Is.EqualTo("Electronics"));
        Assert.That(cat2!.Name, Is.EqualTo("Accessories"));
        Assert.That(cat1.Id, Is.Not.EqualTo(cat2.Id));
    }

    [Test]
    public void TestCategoryNullSafety_GracefullyHandlesMissingCategory()
    {
        // Response payload where category is omitted or null
        const string payloadWithoutCategory = """
        [
            {
                "id": 3,
                "name": "USB Cable",
                "price": 9.99,
                "stock": 200,
                "category": null
            }
        ]
        """;

        var products = JsonSerializer.Deserialize<Product[]>(payloadWithoutCategory, CaseInsensitiveOptions);

        Assert.That(products, Is.Not.Null);
        Assert.That(products!.Length, Is.EqualTo(1));
        Assert.That(products[0].Id, Is.EqualTo(3));
        Assert.That(products[0].Category, Is.Null);
    }

    [Test]
    public void TestCategoryModel_DirectInstantiationAndProperties()
    {
        var category = new Category
        {
            Id = 105,
            Name = "Networking"
        };

        Assert.That(category.Id, Is.EqualTo(105));
        Assert.That(category.Name, Is.EqualTo("Networking"));
    }

    [Test]
    public void TestJsonSerialization_IncludesNestedCategoryStructure()
    {
        var product = new Product
        {
            Id = 5,
            Name = "Monitor",
            Price = 349.99,
            Stock = 12,
            Category = new Category
            {
                Id = 101,
                Name = "Electronics"
            }
        };

        string json = JsonSerializer.Serialize(product);

        Assert.That(json, Does.Contain("\"Category\":{"));
        Assert.That(json, Does.Contain("\"Id\":101"));
        Assert.That(json, Does.Contain("\"Name\":\"Electronics\""));
    }

    [Test]
    public void TestCaseInsensitiveNestedBinding()
    {
        // Lowercase and camelCase nested category fields
        const string camelCaseNested = """
        {
            "id": 10,
            "name": "Tablet",
            "price": 450.00,
            "stock": 15,
            "category": {
                "id": 101,
                "name": "Electronics"
            }
        }
        """;

        var product = JsonSerializer.Deserialize<Product>(camelCaseNested, CaseInsensitiveOptions);

        Assert.That(product, Is.Not.Null);
        Assert.That(product!.Category, Is.Not.Null);
        Assert.That(product.Category!.Id, Is.EqualTo(101));
        Assert.That(product.Category.Name, Is.EqualTo("Electronics"));
    }

    [Test]
    public void TestMalformedNestedCategory_ThrowsJsonException()
    {
        // When category expects an object shape but receives an incompatible scalar string
        const string malformedCategoryPayload = """
        {
            "id": 10,
            "name": "Tablet",
            "price": 450.00,
            "stock": 15,
            "category": "not-an-object"
        }
        """;

        Assert.Throws<JsonException>(() =>
        {
            JsonSerializer.Deserialize<Product>(malformedCategoryPayload, CaseInsensitiveOptions);
        });
    }
}
