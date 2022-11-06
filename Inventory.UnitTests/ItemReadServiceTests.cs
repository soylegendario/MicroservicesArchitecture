using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Inventory.Application.Services;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Persistence;
using Moq;
using Xunit;

namespace Inventory.UnitTests;

public class ItemReadServiceTests
{
    [Theory]
    [AutoData]
    internal async Task GetAllItems_ReturnsAllItems(
        List<Item> expected,
        [Frozen] Mock<InventoryInMemoryContext> context,
        ItemReadService sut)
    {
        // Arrange
        context.Setup(i => i.Items).Returns(expected);

        // Act
        var actual = await sut.GetAllItems();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Theory]
    [AutoData]
    internal async Task NotifyExpiredItems_ReturnsAllExpiredItems(
        [Frozen] Mock<InventoryInMemoryContext> context,
        ItemReadService sut)
    {
        // Arrange
        var expected = new List<Item>
        {
            new() { Id = Guid.NewGuid(), Name = "Item 1", ExpirationDate = DateTime.Now.AddDays(-1)},
            new() { Id = Guid.NewGuid(), Name = "Item 2", ExpirationDate = DateTime.Now.AddDays(-2)},
            new() { Id = Guid.NewGuid(), Name = "Item 3", ExpirationDate = DateTime.Now.AddDays(-3)},
            new() { Id = Guid.NewGuid(), Name = "Item 4", ExpirationDate = DateTime.Now.AddDays(1)},
            new() { Id = Guid.NewGuid(), Name = "Item 5", ExpirationDate = DateTime.Now.AddDays(2)},
            new() { Id = Guid.NewGuid(), Name = "Item 6", ExpirationDate = DateTime.Now.AddDays(3)}
        };
        context.Setup(i => i.Items).Returns(expected);

        // Act
        var expiredItems = await sut.NotifyExpiredItems();

        // Assert
        Assert.Equal(1, expiredItems);
    }
}