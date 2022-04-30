using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Inventory.Application.Mappers.Items;
using Inventory.Application.Services;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Helpers.Cqrs.Queries;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Queries;
using Inventory.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Inventory.UnitTests;

[TestFixture]
public class ItemReadServiceTests
{
    private Mock<InventoryInMemoryContext> _inventoryInMemoryContextMock = null!;
    private ItemReadService _itemReadService = null!;

    [SetUp]
    public void SetUp()
    {
        _inventoryInMemoryContextMock = new Mock<InventoryInMemoryContext>();
        var itemRepository = new ItemInMemoryRepository(Mock.Of<ILogger<ItemInMemoryRepository>>(),
            _inventoryInMemoryContextMock.Object);

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(x => x.GetService(typeof(IQueryHandler<GetAllItemsQuery, IEnumerable<Item>>)))
            .Returns(new GetAllItemsQueryHandler(Mock.Of<ILogger<GetAllItemsQueryHandler>>(),
                itemRepository));
        
        _itemReadService = new ItemReadService(Mock.Of<ILogger<ItemReadService>>(),
            new QueryDispatcher(serviceProvider.Object), new ItemMapper(Mock.Of<ILogger<ItemMapper>>()));
    }

    [Test]
    public async Task GetAllItems_ReturnsAllItems()
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
        var actual = await _itemReadService.GetAllItems();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}