using System;
using System.Linq;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Repository;
using NUnit.Framework;

namespace Inventory.UnitTests;

[TestFixture]
public class ItemRepositoryTests
{
    private IItemRepository _itemRepository = null!;
    private IInventoryContext _inventoryContext = null!;
    private Item _item = null!;

    [SetUp]
    public void SetUp()
    {
        _inventoryContext = new InMemoryInventoryContext();
        _itemRepository = new ItemRepository(_inventoryContext);
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
        Assert.AreEqual(1, _inventoryContext.Items().Count);
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
    public void CanRemoveItem()
    {
        // Arrange
        _inventoryContext.Items().Add(_item);
        
        // Act
        _itemRepository.RemoveItemByName("Test Item");
    
        // Assert
        Assert.AreEqual(0, _inventoryContext.Items().Count);
    }
}