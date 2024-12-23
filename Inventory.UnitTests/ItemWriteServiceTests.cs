using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using CrossCutting.Cqrs.Commands;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Inventory.Application.Commands.Item;
using Inventory.Application.Dto;
using Inventory.Application.Exceptions;
using Inventory.Application.Services;
using Inventory.Domain.Items;
using Moq;
using Xunit;

namespace Inventory.UnitTests;

public class ItemWriteServiceTests
{
    [Theory]
    [AutoMoqData]
    internal async Task AddItem_AddsItem(
        ItemDto item,
        ItemWriteService sut)
    {
        // Arrange

        // Act
        var exception = await Record.ExceptionAsync(() => sut.AddItemAsync(item));
        
        // Assert
        Assert.Null(exception);
    }

    [Theory]
    [AutoMoqData]
    internal async Task GivenANotValidItem_WhenAddItemIsCalled_ShouldThrowsValidationException(
        ItemDto item,
        [Frozen] Mock<IValidator<ItemDto>> validator,
        ItemWriteService sut)
    {
        // Arrange
        item.Name = "";
        validator.Setup(mock =>
                mock.Validate(item))
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure("", "")
            }));
        
        // Act
        var exception = await Record.ExceptionAsync(() => sut.AddItemAsync(item));

        // Assert
        exception.Should().BeOfType<ValidationException>();
    }
    
    [Theory]
    [AutoMoqData]
    internal async Task RemoveItemByName_RemovesItem(
        List<Item> items,
        ItemWriteService sut)
    {
        // Arrange
        
        // Assert
        var exception = await Record.ExceptionAsync(() => sut.RemoveItemByNameAsync(items[0].Name));
        
        // Assert
        Assert.Null(exception);
    }
    
    [Theory]
    [AutoMoqData]
    internal async Task RemoveItemByName_ThrowsException_WhenItemNotFound(
        [Frozen] Mock<ICommandDispatcher> commandDispatcher,
        ItemWriteService sut)
    {
        // Arrange
        commandDispatcher.Setup(mock =>
                mock.DispatchAsync(It.IsAny<RemoveItemByNameCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ItemNotFoundException());
        
        // Act 
        var exception = await Record.ExceptionAsync(() => sut.RemoveItemByNameAsync("Non-existent item"));
        
        // Assert
        exception.Should().BeOfType<ItemNotFoundException>();
    }
}