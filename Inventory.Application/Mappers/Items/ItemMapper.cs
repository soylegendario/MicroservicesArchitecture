using System.Runtime.CompilerServices;
using Inventory.Application.Dto;
using Inventory.Domain.Items;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")] 
[assembly: InternalsVisibleTo("Inventory.UnitTests")]
namespace Inventory.Application.Mappers.Items;


internal class ItemMapper : IItemMapper
{
    private readonly ILogger<ItemMapper> _logger;

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