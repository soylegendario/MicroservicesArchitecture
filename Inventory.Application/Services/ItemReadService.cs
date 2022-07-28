using Inventory.Application.Contracts;
using Inventory.Application.Dto;
using Inventory.Application.Mappers.Items;
using Inventory.CrossCutting.Cqrs.Queries;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Events;
using Inventory.Infrastructure.Helpers.Events;
using Inventory.Infrastructure.Queries;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Services;

public class ItemReadService : IItemReadService
{
    private readonly ILogger<ItemReadService> _logger;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly IItemMapper _itemMapper;
    private readonly IEventBus _eventBus;

    public ItemReadService(ILogger<ItemReadService> logger,
        IQueryDispatcher queryDispatcher,
        IItemMapper itemMapper,
        IEventBus eventBus)
    {
        _logger = logger;
        _queryDispatcher = queryDispatcher;
        _itemMapper = itemMapper;
        _eventBus = eventBus;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<int> NotifyExpiredItems()
    {
        try
        {
            _logger.LogInformation("Notifying expired items");
            var query = new GetItemsByExpirationDateQuery
            {
                ExpirationDate = DateTime.UtcNow.AddDays(-1)
            };
            var expiredItems =
                (await _queryDispatcher.DispatchAsync<GetItemsByExpirationDateQuery, IEnumerable<Item>>(query)).ToArray();
            foreach (var item in expiredItems)
            {
                var eventData = new ItemExpiredEvent
                {
                    Name = item.Name,
                    ExpirationDate = item.ExpirationDate
                };
                _eventBus.Publish(eventData);
            }
            return expiredItems.Length;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error notifying expired items");
            throw;
        }
    }
}