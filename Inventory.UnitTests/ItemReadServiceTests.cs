using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using CrossCutting.Cqrs.Queries;
using FluentAssertions;
using Inventory.Application.Dto;
using Inventory.Application.Exceptions;
using Inventory.Application.Mappers.Items;
using Inventory.Application.Queries.Item;
using Inventory.Application.Services;
using Inventory.Domain.Items;
using Moq;
using Xunit;

namespace Inventory.UnitTests;

public class ItemReadServiceTests
{
    [Theory]
    [AutoMoqData]
    internal async Task GetAllItems_ReturnsAllItems(
        List<ItemDto> expected,
        [Frozen] Mock<IItemMapper> mapper,
        ItemReadService sut)
    {
        // Arrange
        mapper.Setup(mock => mock.Map(It.IsAny<IEnumerable<Item>>())).Returns(expected);

        // Act
        var actual = await sut.GetAllItems();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Theory]
    [AutoMoqData]
    internal async Task NotifyExpiredItems_ReturnsAllExpiredItems(
        [Frozen] Mock<IQueryDispatcher> queryDispatcher,
        ItemReadService sut)
    {
        // Arrange
        var expected = new List<Item>
        {
            new() { Id = Guid.NewGuid(), Name = "Item 1", ExpirationDate = DateTime.Now.AddDays(-1)}
        };
        queryDispatcher.Setup(mock =>
                mock.DispatchAsync<GetItemsByExpirationDateQuery, IEnumerable<Item>>(
                    It.IsAny<GetItemsByExpirationDateQuery>(), 
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var expiredItems = await sut.NotifyExpiredItems();

        // Assert
        Assert.Equal(1, expiredItems);
    }
    
    [Theory]
    [AutoMoqData]
    internal async Task GetItemById_ItemNotFound_ThrowsException(
        Guid itemId,
        [Frozen] Mock<IQueryDispatcher> queryDispatcher,
        ItemReadService sut)
    {
        // Arrange
        var query = new GetItemByIdQuery { Id = itemId };
        queryDispatcher.Setup(mock =>
                mock.DispatchAsync<GetItemByIdQuery, Item>(
                    It.IsAny<GetItemByIdQuery>(),
                    It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ItemNotFoundException($"Item with ID {itemId} not found"));

        // Act
        Func<Task> act = async () => await sut.GetItemByIdAsync(query.Id);

        // Assert
        await act.Should().ThrowAsync<ItemNotFoundException>()
            .WithMessage($"Item with ID {itemId} not found");
    }
}