using Inventory.Application.Dto;
using Inventory.Domain.Items;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Mappers.Items;

public class ItemMapper : IItemMapper
{
    public readonly ILogger<ItemMapper> _logger;

    public ItemMapper(ILogger<ItemMapper> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public ItemDto Map(Item item)
    {
        _logger.LogInformation("Mapping item to dto");
        var itemDto = new ItemDto
        {
            Id = item.Id,
            Name = item.Name,
            ExpirationDate = item.ExpirationDate
        };
        return itemDto;
    }

    /// <inheritdoc />
    public IEnumerable<ItemDto> Map(IEnumerable<Item> items)
    {
        _logger.LogInformation("Mapping items to dto");
        return items.Select(Map).ToList();
    }

    /// <inheritdoc />
    public Item Map(ItemDto itemDto)
    {
        _logger.LogInformation("Mapping dto to item");
        var item = new Item
        {
            Id = itemDto.Id ?? Guid.Empty,
            Name = itemDto.Name,
            ExpirationDate = itemDto.ExpirationDate
        };
        return item;
    }
}