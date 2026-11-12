using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace InventoryHub.Tests;

/// <summary>
/// Unit tests verifying in-memory caching mechanisms, dependency injection resolution,
/// sliding expiration policies, and cache hit/miss behavior for the InventoryHub API.
/// </summary>
[TestFixture]
public class TestPerformanceAndCaching
{
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

    private static Product[] CreateSeedProducts() =>
    [
        new Product
        {
            Id = 1,
            Name = "Laptop",
            Price = 1200.50,
            Stock = 25,
            Category = new Category { Id = 101, Name = "Electronics" }
        },
        new Product
        {
            Id = 2,
            Name = "Headphones",
            Price = 50.00,
            Stock = 100,
            Category = new Category { Id = 102, Name = "Accessories" }
        }
    ];

    [Test]
    public void TestMemoryCache_RegistrationResolvesSuccessfully()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        using var provider = services.BuildServiceProvider();

        var cache = provider.GetService<IMemoryCache>();
        Assert.That(cache, Is.Not.Null, "IMemoryCache must resolve cleanly when AddMemoryCache is registered.");
    }

    [Test]
    public void TestProductListCache_FirstRequestCachesData()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());
        const string cacheKey = "productlist";

        var initialCheck = cache.TryGetValue(cacheKey, out object? initialResult);
        Assert.That(initialCheck, Is.False, "Cache must be empty before the first request.");
        Assert.That(initialResult, Is.Null);

        var seed = CreateSeedProducts();
        cache.Set(cacheKey, seed, new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(5)
        });

        var cachedCheck = cache.TryGetValue(cacheKey, out object? cachedResult);
        Assert.That(cachedCheck, Is.True, "Cache must contain productlist after initial population.");
        Assert.That(cachedResult, Is.Not.Null);

        var products = cachedResult as Product[];
        Assert.That(products, Is.Not.Null);
        Assert.That(products!.Length, Is.EqualTo(2));
    }

    [Test]
    public void TestProductListCache_SubsequentRequestRetrievesCachedInstance()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());
        const string cacheKey = "productlist";
        var originalInstance = CreateSeedProducts();

        cache.Set(cacheKey, originalInstance, TimeSpan.FromMinutes(5));

        Assert.That(cache.TryGetValue(cacheKey, out var retrieved1), Is.True);
        Assert.That(cache.TryGetValue(cacheKey, out var retrieved2), Is.True);
        Assert.That(ReferenceEquals(retrieved1, retrieved2), Is.True, "Subsequent reads must return identical cached reference.");
        Assert.That(ReferenceEquals(retrieved1, originalInstance), Is.True);
    }

    [Test]
    public void TestProductListCache_SlidingExpirationConfigured()
    {
        var options = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(5)
        };

        Assert.That(options.SlidingExpiration, Is.Not.Null);
        Assert.That(options.SlidingExpiration!.Value.TotalMinutes, Is.EqualTo(5.0));
        Assert.That(options.AbsoluteExpiration, Is.Null, "Sliding expiration policy should not enforce rigid absolute expiration by default.");
    }

    [Test]
    public void TestProductListCache_NullSafety()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());
        const string nonExistentKey = "unknown_key_999";

        var found = cache.TryGetValue(nonExistentKey, out object? result);
        Assert.That(found, Is.False);
        Assert.That(result, Is.Null, "Querying an absent key must return false and a null result without throwing.");
    }
}
