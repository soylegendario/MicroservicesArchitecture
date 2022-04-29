using Inventory.Application.Dto;
using Inventory.Domain.Items;

namespace Inventory.Application.Mappers.Items;

public interface IItemMapper
{
    ItemDto Map(Item item);
    IEnumerable<ItemDto> Map(IEnumerable<Item> items);
}