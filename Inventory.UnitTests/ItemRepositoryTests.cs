using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Inventory.CrossCutting.Exceptions;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Inventory.UnitTests;

public class ItemRepositoryTests
{
    private readonly Item _item = new Fixture().Create<Item>();
    
    [Theory]
    [AutoMoqData]
    internal void CanAddItem(ItemRepository sut)
    {
        // Act
        sut.AddItem(_item);
        
        // Assert
        var items = sut.GetAllItems();
        items.Should().HaveCount(1);
    }

    [Theory]
    [AutoMoqData]
    internal void CanGetAllItems(ItemRepository sut)
    {
        // Arrange
        sut.AddItem(_item);
        
        // Act
        var items = sut.GetAllItems();

        // Assert
        items.Should().HaveCount(1);
    }

    [Theory]
    [AutoMoqData]
    internal void GetItemsByExpirationDate_ReturnsItemsWithGivenExpirationDate(ItemRepository sut)
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
    [AutoMoqData]
    internal void CanRemoveItem(
        ItemRepository sut)
    {
        // Arrange
        sut.AddItem(_item);
        
        // Act
        sut.RemoveItemByName(_item.Name);
    
        // Assert
        var items = sut.GetAllItems();
        items.Should().BeEmpty();
    }
    
    [Theory]
    [AutoMoqData]
    internal void TryRemoveItemByName_WhenItemDoesNotExist_ThrowsItemNotFoundException(ItemRepository sut)
    {
        Assert.Throws<ItemNotFoundException>(() => sut.RemoveItemByName("Non-existent Item"));
    }
    
}