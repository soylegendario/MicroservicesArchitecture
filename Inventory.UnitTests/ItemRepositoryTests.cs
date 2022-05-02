using System;
using System.Linq;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Exceptions;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Inventory.UnitTests;

[TestFixture]
public class ItemRepositoryTests
{
    private IItemRepository _itemRepository = null!;
    private InventoryInMemoryContext _inventoryContext = null!;
    private Item _item = null!;

    [SetUp]
    public void SetUp()
    {
        _inventoryContext = new InventoryInMemoryContext();
        var logger = Mock.Of<ILogger<ItemInMemoryRepository>>();
        _itemRepository = new ItemInMemoryRepository(logger, _inventoryContext);
        _item = new Item
        {
            Name = "Test Item", 
            ExpirationDate = DateTime.UtcNow.AddDays(10)
        };
    }

    [Test]
    public void CanAddItem()
    {
        // Act
        _itemRepository.AddItem(_item);
        
        // Assert
        Assert.AreEqual(1, _inventoryContext.Items.Count);
        Assert.NotNull(_item.Id);
        Assert.AreEqual(DateTime.UtcNow.AddDays(10).Date, _item.ExpirationDate.Date);
    }

    [Test]
    public void CanGetAllItems()
    {
        // Act
        _itemRepository.AddItem(_item);
        var items = _itemRepository.GetAllItems();

        // Assert
        Assert.AreEqual(1, items.Count());
    }

    [Test]
    public void GetItemsByExpirationDate_ReturnsItemsWithGivenExpirationDate()
    {
        // Arrange
        _itemRepository.AddItem(_item);
        var expirationDate = DateTime.UtcNow.AddDays(1);
        var item = new Item { ExpirationDate = expirationDate };
        _itemRepository.AddItem(item);

        // Act
        var items = _itemRepository.GetItemsByExpirationDate(expirationDate).ToArray();

        // Assert
        Assert.AreEqual(1, items.Length);
        Assert.AreEqual(expirationDate, items.First().ExpirationDate);
    }
    
    [Test]
    public void CanRemoveItem()
    {
        // Arrange
        _inventoryContext.Items.Add(_item);
        
        // Act
        _itemRepository.RemoveItemByName("Test Item");
    
        // Assert
        Assert.AreEqual(0, _inventoryContext.Items.Count);
    }
    
    [Test]
    public void TryRemoveItemByName_WhenItemDoesNotExist_ThrowsItemNotFoundException()
    {
        Assert.Throws<ItemNotFoundException>(() => _itemRepository.RemoveItemByName("Non-existent Item"));
    }
    
}