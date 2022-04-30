using System;
using System.Collections.Generic;
using Inventory.Application.Services;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Exceptions;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Inventory.UnitTests;

[TestFixture]
public class ItemServiceTests
{
    private Mock<InventoryInMemoryContext> _inventoryInMemoryContextMock = null!;
    private ItemInMemoryRepository _itemRepository = null!;
    private ItemService _itemService = null!;

    [SetUp]
    public void SetUp()
    {
        _inventoryInMemoryContextMock = new Mock<InventoryInMemoryContext>();
        _itemRepository = new ItemInMemoryRepository(Mock.Of<ILogger<ItemInMemoryRepository>>(),
            _inventoryInMemoryContextMock.Object);
        _itemService = new ItemService(Mock.Of<ILogger<ItemService>>(), _itemRepository);
    }

    [Test]
    public void GetAllItems_ReturnsAllItems()
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
        var actual = _itemService.GetAllItems();

        // Assert
        Assert.AreEqual(expected, actual);
    }
    
    [Test]
    public void AddItem_AddsItem()
    {
        // Arrange
        _inventoryInMemoryContextMock.Setup(i => i.Items).Returns(new List<Item>());
        var item = new Item { Name = "Item 4"};

        // Act
        _itemService.AddItem(item);

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
        Assert.DoesNotThrow(() => _itemService.RemoveItemByName("Item 1"));
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
        Assert.Throws<ItemNotFoundException>(() => _itemService.RemoveItemByName("Non-existent item"));
    }
}