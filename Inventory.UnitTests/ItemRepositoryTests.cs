using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Inventory.CrossCutting.Exceptions;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Inventory.UnitTests;

public class ItemRepositoryTests
{
    private readonly Item _item = new Fixture().Create<Item>();
    
    [Theory]
    [AutoData]
    internal void CanAddItem(ItemInMemoryRepository sut)
    {
        // Act
        sut.AddItem(_item);
        
        // Assert
        var items = sut.GetAllItems();
        items.Should().HaveCount(1);
    }

    [Theory]
    [AutoData]
    internal void CanGetAllItems(ItemInMemoryRepository sut)
    {
        // Arrange
        sut.AddItem(_item);
        
        // Act
        var items = sut.GetAllItems();

        // Assert
        items.Should().HaveCount(1);
    }

    [Theory]
    [AutoData]
    internal void GetItemsByExpirationDate_ReturnsItemsWithGivenExpirationDate(ItemInMemoryRepository sut)
    {
        // Arrange
        sut.AddItem(_item);
        var expirationDate = DateTime.UtcNow.AddDays(1);
        var item = new Item { ExpirationDate = expirationDate };
        sut.AddItem(item);

        // Act
        var items = sut.GetItemsByExpirationDate(expirationDate).ToArray();

        // Assert
        items.Should().HaveCount(1);
        items[0].ExpirationDate.Should().Be(expirationDate);
    }
    
    [Theory]
    [AutoData]
    internal void CanRemoveItem(ItemInMemoryRepository sut)
    {
        // Arrange
        sut.AddItem(_item);
        
        // Act
        sut.RemoveItemByName("Test Item");
    
        // Assert
        var items = sut.GetAllItems();
        items.Should().BeEmpty();
    }
    
    [Theory]
    [AutoData]
    internal void TryRemoveItemByName_WhenItemDoesNotExist_ThrowsItemNotFoundException(ItemInMemoryRepository sut)
    {
        Assert.Throws<ItemNotFoundException>(() => sut.RemoveItemByName("Non-existent Item"));
    }
    
}