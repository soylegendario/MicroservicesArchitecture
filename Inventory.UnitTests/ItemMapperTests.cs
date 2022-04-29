using System;
using System.Linq;
using Inventory.Application.Mappers.Items;
using Inventory.Domain.Items;
using NUnit.Framework;

namespace Inventory.UnitTests;

[TestFixture]
public class ItemMapperTests
{
    private ItemMapper _itemMapperTests = null!;

    [SetUp]
    public void SetUp()
    {
        _itemMapperTests = new ItemMapper();
    }

    [Test]
    public void ShouldMapItem()
    {
        var item = new Item
        {
            Id = Guid.NewGuid(),
            Name = "Item 1",
            ExpirationDate = DateTime.UtcNow
        };

        var itemDto = _itemMapperTests.Map(item);

        Assert.That(itemDto.Id, Is.EqualTo(item.Id));
        Assert.That(itemDto.Name, Is.EqualTo(item.Name));
        Assert.That(itemDto.ExpirationDate, Is.EqualTo(item.ExpirationDate));
    }

    [Test]
    public void ShouldMapItems()
    {
        var items = new[]
        {
            new Item
            {
                Id = Guid.NewGuid(),
                Name = "Item 1",
                ExpirationDate = DateTime.UtcNow
            },
            new Item
            {
                Id = Guid.NewGuid(),
                Name = "Item 2",
                ExpirationDate = DateTime.UtcNow
            }
        };
        
        var itemsDto = _itemMapperTests.Map(items);
        
        Assert.That(itemsDto.Count(), Is.EqualTo(items.Length));
    }
}