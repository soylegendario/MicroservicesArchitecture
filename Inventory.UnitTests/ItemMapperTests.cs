using FluentAssertions;
using Inventory.Application.Mappers.Items;
using Inventory.Domain.Items;
using Xunit;

namespace Inventory.UnitTests;

public class ItemMapperTests
{
    public ItemMapperTests()
    {
    }
    
    [Theory]
    [AutoMoqData]
    internal void ShouldMapItem(Item item, ItemMapper sut)
    {
        var itemDto = sut.Map(item);

        Assert.Equal(item.Id, itemDto.Id);
        Assert.Equal(item.Id, itemDto.Id);
        Assert.Equal(item.Name, itemDto.Name);
        Assert.Equal(item.ExpirationDate, itemDto.ExpirationDate);
    }

    [Theory]
    [AutoMoqData]
    internal void ShouldMapItems(Item[] items, ItemMapper sut)
    {
        var itemsDto = sut.Map(items);

        itemsDto.Should().BeEquivalentTo(items);
    }
}