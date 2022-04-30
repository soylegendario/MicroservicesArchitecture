using Inventory.Application.Contracts;
using Inventory.Application.Dto;
using Inventory.Application.Mappers.Items;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Helpers.Cqrs.Queries;
using Inventory.Infrastructure.Queries;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Services;

public class ItemReadService : IItemReadService
{
    private readonly ILogger<ItemReadService> _logger;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly IItemMapper _itemMapper;

    public ItemReadService(ILogger<ItemReadService> logger, IQueryDispatcher queryDispatcher, IItemMapper itemMapper)
    {
        _logger = logger;
        _queryDispatcher = queryDispatcher;
        _itemMapper = itemMapper;
    }

    public async Task<IEnumerable<ItemDto>> GetAllItems()
    {
        try
        {
            _logger.LogInformation("Getting all items");
            var items = await _queryDispatcher.DispatchAsync<GetAllItemsQuery, IEnumerable<Item>>(new GetAllItemsQuery());
            return _itemMapper.Map(items);

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting all items");
            throw;
        }
    }
}