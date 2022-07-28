using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Inventory.Application.Mappers.Items;
using Inventory.Application.Services;
using Inventory.CrossCutting.Cqrs.Queries;
using Inventory.CrossCutting.Events;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Queries;
using Inventory.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Inventory.UnitTests;

[TestFixture]
public class ItemReadServiceTests
{
    private Mock<InventoryInMemoryContext> _inventoryInMemoryContextMock = null!;
    private ItemReadService _itemReadService = null!;

    [SetUp]
    public void SetUp()
    {
        _inventoryInMemoryContextMock = new Mock<InventoryInMemoryContext>();
        var itemRepository = new ItemInMemoryRepository(Mock.Of<ILogger<ItemInMemoryRepository>>(),
            _inventoryInMemoryContextMock.Object);

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(x => x.GetService(typeof(IQueryHandler<GetAllItemsQuery, IEnumerable<Item>>)))
            .Returns(new GetAllItemsQueryHandler(Mock.Of<ILogger<GetAllItemsQueryHandler>>(),
                itemRepository));
        serviceProvider.Setup(x => x.GetService(typeof(IQueryHandler<GetItemsByExpirationDateQuery, IEnumerable<Item>>)))
            .Returns(new GetItemsByExpirationDateQueryHandler(Mock.Of<ILogger<GetItemsByExpirationDateQueryHandler>>(),
                itemRepository));
        
        _itemReadService = new ItemReadService(Mock.Of<ILogger<ItemReadService>>(),
            new QueryDispatcher(serviceProvider.Object), new ItemMapper(Mock.Of<ILogger<ItemMapper>>()),
            Mock.Of<IEventBus>());
    }

    [Test]
    public async Task GetAllItems_ReturnsAllItems()
    {
        // Arrange
        var expected = new List<Item>
        {
            new() { Id = Guid.NewGuid(), Name = "Item 1"},
            new() { Id = Guid.NewGuid(), Name = "Item 2"},
            new() { Id = Guid.NewGuid(), Name = "Item 3"}
        };
        _inventoryInMemoryContextMock.Setup(i => i.Items).Returns(expected);

        // Act
        var actual = await _itemReadService.GetAllItems();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Test]
    public async Task NotifyExpiredItems_ReturnsAllExpiredItems()
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
        _inventoryInMemoryContextMock.Setup(i => i.Items).Returns(expected);

        // Act
        var expiredItems = await _itemReadService.NotifyExpiredItems();

        // Assert
        Assert.AreEqual(1, expiredItems);
    }
}