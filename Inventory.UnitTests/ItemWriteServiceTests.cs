using System;
using System.Collections.Generic;
using Inventory.Application.Services;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Commands;
using Inventory.Infrastructure.Exceptions;
using Inventory.Infrastructure.Helpers.Cqrs.Commands;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Inventory.UnitTests;

[TestFixture]
public class ItemWriteServiceTests
{
    private Mock<InventoryInMemoryContext> _inventoryInMemoryContextMock = null!;
    private ItemWriteService _itemWriteService = null!;

    [SetUp]
    public void SetUp()
    {
        _inventoryInMemoryContextMock = new Mock<InventoryInMemoryContext>();
        var itemRepository = new ItemInMemoryRepository(Mock.Of<ILogger<ItemInMemoryRepository>>(),
            _inventoryInMemoryContextMock.Object);

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(x => x.GetService(typeof(ICommandHandler<AddItemCommand>)))
            .Returns(new AddItemCommandHandler(Mock.Of<ILogger<AddItemCommandHandler>>(),
                itemRepository));
        serviceProvider.Setup(x => x.GetService(typeof(ICommandHandler<RemoveItemByNameCommand>)))
            .Returns(new RemoveItemByNameCommandHandler(Mock.Of<ILogger<RemoveItemByNameCommandHandler>>(),
                itemRepository));
        
        _itemWriteService = new ItemWriteService(Mock.Of<ILogger<ItemWriteService>>(),
            new CommandDispatcher(serviceProvider.Object));
    }
    
    [Test]
    public void AddItem_AddsItem()
    {
        // Arrange
        _inventoryInMemoryContextMock.Setup(i => i.Items).Returns(new List<Item>());
        var item = new Item { Name = "Item 4"};

        // Act
        _itemWriteService.AddItem(item);

        // Assert
        Assert.NotNull(item.Id);
    }

    [Test]
    public void RemoveItemByName_RemovesItem()
    {
        // Arrange
        var expected = new List<Item>
        {
            new() { Id = Guid.NewGuid(), Name = "Item 1"}
        };
        _inventoryInMemoryContextMock.Setup(i => i.Items).Returns(expected);
        
        // Act&Assert
        Assert.DoesNotThrow(() => _itemWriteService.RemoveItemByName("Item 1"));
    }
    
    [Test]
    public void RemoveItemByName_ThrowsException_WhenItemNotFound()
    {
        // Arrange
        var expected = new List<Item>
        {
            new() { Id = Guid.NewGuid(), Name = "Item 1"}
        };
        _inventoryInMemoryContextMock.Setup(i => i.Items).Returns(expected);
        
        // Act&Assert
        Assert.Throws<ItemNotFoundException>(() => _itemWriteService.RemoveItemByName("Non-existent item"));
    }
}