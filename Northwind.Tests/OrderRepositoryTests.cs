using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Northwind.Domain.Entities;
using Northwind.Infrastructure.Data;
using Northwind.Infrastructure.Repositories;

namespace Northwind.Tests;

[TestClass]
public class OrderRepositoryTests
{
    private NorthwindContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<NorthwindContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new NorthwindContext(options);
    }

    [TestMethod]
    public async Task GetAllAsync_ReturnsAllOrders()
    {
        using var context = CreateContext();
        context.Orders.AddRange(
            new Order { OrderId = 1, CustomerId = "ALFKI" },
            new Order { OrderId = 2, CustomerId = "VINET" }
        );
        await context.SaveChangesAsync();
        var repo = new OrderRepository(context);
        var result = await repo.GetAllAsync();
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public async Task GetByIdAsync_ReturnsCorrectOrder()
    {
        using var context = CreateContext();
        context.Orders.Add(new Order { OrderId = 1, CustomerId = "ALFKI" });
        await context.SaveChangesAsync();
        var repo = new OrderRepository(context);
        var result = await repo.GetByIdAsync(1);
        Assert.IsNotNull(result);
        Assert.AreEqual("ALFKI", result.CustomerId);
    }

    [TestMethod]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        using var context = CreateContext();
        var repo = new OrderRepository(context);
        var result = await repo.GetByIdAsync(999);
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task CreateAsync_AddsOrder()
    {
        using var context = CreateContext();
        var repo = new OrderRepository(context);
        var order = new Order { OrderId = 1, CustomerId = "ALFKI" };
        var result = await repo.CreateAsync(order);
        Assert.AreEqual(1, context.Orders.Count());
        Assert.AreEqual("ALFKI", result.CustomerId);
    }

    [TestMethod]
    public async Task DeleteAsync_RemovesOrder()
    {
        using var context = CreateContext();
        context.Orders.Add(new Order { OrderId = 1, CustomerId = "ALFKI" });
        await context.SaveChangesAsync();
        var repo = new OrderRepository(context);
        await repo.DeleteAsync(1);
        Assert.AreEqual(0, context.Orders.Count());
    }

    [TestMethod]
    public async Task UpdateAsync_ModifiesOrder()
    {
        using var context = CreateContext();
        context.Orders.Add(new Order { OrderId = 1, CustomerId = "ALFKI", ShipCity = "Berlin" });
        await context.SaveChangesAsync();
        var repo = new OrderRepository(context);
        var order = await repo.GetByIdAsync(1);
        order!.ShipCity = "London";
        await repo.UpdateAsync(order);
        var updated = await repo.GetByIdAsync(1);
        Assert.AreEqual("London", updated!.ShipCity);
    }
}