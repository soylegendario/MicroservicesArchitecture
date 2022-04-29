using Inventory.Application.Dto;
using Inventory.Domain.Items;

namespace Inventory.Application.Mappers.Items;

public class ItemMapper : IItemMapper
{
    public ItemDto Map(Item item)
    {
        var itemDto = new ItemDto
        {
            Id = item.Id,
            Name = item.Name,
            ExpirationDate = item.ExpirationDate
        };
        return itemDto;
    }

    public IEnumerable<ItemDto> Map(IEnumerable<Item> items)
    {
        return items.Select(Map).ToList();
    }
}