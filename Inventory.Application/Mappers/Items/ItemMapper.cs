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

    public IEnumerable<ItemDto> Map(IEnumerable<Item> items)
    {
        _logger.LogInformation("Mapping items to dto");
        return items.Select(Map).ToList();
    }
}