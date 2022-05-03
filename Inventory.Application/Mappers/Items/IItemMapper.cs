using Inventory.Application.Dto;
using Inventory.Domain.Items;

namespace Inventory.Application.Mappers.Items;

public interface IItemMapper
{
    /// <summary>
    /// Map a <see cref="Item"/> to a <see cref="ItemDto"/>.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    ItemDto Map(Item item);
    
    /// <summary>
    /// Map a enumerable of <see cref="ItemDto"/> to a enumerable of <see cref="Item"/>.
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    IEnumerable<ItemDto> Map(IEnumerable<Item> items);
    
    /// <summary>
    /// Map a <see cref="ItemDto"/> to a <see cref="Item"/>.
    /// </summary>
    /// <param name="itemDto"></param>
    /// <returns></returns>
    Item Map(ItemDto itemDto);
}