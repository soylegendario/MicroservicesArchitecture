using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentValidation;
using Inventory.Application.Dto;
using Inventory.Application.Services;
using Inventory.CrossCutting.Exceptions;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Persistence;
using Moq;
using Xunit;

namespace Inventory.UnitTests;

public class ItemWriteServiceTests
{
    [Theory]
    [AutoData]
    internal async Task AddItem_AddsItem(
        ItemDto item,
        [Frozen] Mock<InventoryInMemoryContext> context,
        ItemWriteService sut)
    {
        // Arrange
        context.Setup(i => i.Items).Returns(new List<Item>());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.AddItem(item));
        
        // Assert
        Assert.Null(exception);
    }

    [Theory]
    [AutoData]
    internal void AddItem_GivenEmptyName_ThrowsValidationException(
        ItemDto item,
        [Frozen] Mock<InventoryInMemoryContext> context,
        ItemWriteService sut)
    {
        // Arrange
        context.Setup(i => i.Items).Returns(new List<Item>());
        item.Name = "";
        
        // Act
        var exception = Record.ExceptionAsync(() => sut.AddItem(item));

        // Assert
        exception.Should().BeOfType<ValidationException>();
    }
    
    [Theory]
    [AutoData]
    internal async Task AddItem_GivenExpirationDateInThePast_ThrowsValidationException(
        ItemDto item,
        [Frozen] Mock<InventoryInMemoryContext> context,
        ItemWriteService sut)
    {
        // Arrange
        context.Setup(i => i.Items).Returns(new List<Item>());
        item.ExpirationDate = DateTime.Now.AddDays(-1);

        // Act
        var exception = await Record.ExceptionAsync(() => sut.AddItem(item));
        
        // Act&Assert
        exception.Should().BeOfType<ValidationException>();
    }
    

    [Theory]
    [AutoData]
    internal async Task RemoveItemByName_RemovesItem(
        List<Item> items,
        [Frozen] Mock<InventoryInMemoryContext> context,
        ItemWriteService sut)
    {
        // Arrange
        context.Setup(i => i.Items).Returns(items);
        
        // Assert
        var exception = await Record.ExceptionAsync(() => sut.RemoveItemByName(items[0].Name));
        
        // Assert
        Assert.Null(exception);
    }
    
    [Theory]
    [AutoData]
    internal void RemoveItemByName_ThrowsException_WhenItemNotFound(
        List<Item> items,
        [Frozen] Mock<InventoryInMemoryContext> context,
        ItemWriteService sut)
    {
        // Arrange
        context.Setup(i => i.Items).Returns(items);
        
        // Act 
        var exception = Record.ExceptionAsync(() => sut.RemoveItemByName("Non-existent item"));
        
        // Assert
        exception.Should().BeOfType<ItemNotFoundException>();
    }
}