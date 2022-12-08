using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Inventory.Application.Dto;
using Inventory.Application.Mappers.Items;
using Inventory.Application.Services;
using Inventory.CrossCutting.Cqrs.Queries;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Queries;
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
}