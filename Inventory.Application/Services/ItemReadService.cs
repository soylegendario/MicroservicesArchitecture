using Inventory.Application.Contracts;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Helpers.Cqrs.Queries;
using Inventory.Infrastructure.Queries;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Services;

public class ItemReadService : IItemReadService
{
    private readonly ILogger<ItemReadService> _logger;
    private readonly IQueryDispatcher _queryDispatcher;

    public ItemReadService(ILogger<ItemReadService> logger, IQueryDispatcher queryDispatcher)
    {
        _logger = logger;
        _queryDispatcher = queryDispatcher;
    }

    public Task<IEnumerable<Item>> GetAllItems()
    {
        try
        {
            _logger.LogInformation("Getting all items");
            return _queryDispatcher.DispatchAsync<GetAllItemsQuery, IEnumerable<Item>>(new GetAllItemsQuery());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting all items");
            throw;
        }
    }
}