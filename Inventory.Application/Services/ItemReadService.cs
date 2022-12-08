using System.Runtime.CompilerServices;
using Inventory.Application.Contracts;
using Inventory.Application.Dto;
using Inventory.Application.Mappers.Items;
using Inventory.CrossCutting.Cqrs.Queries;
using Inventory.CrossCutting.Events;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Events;
using Inventory.Infrastructure.Queries;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")] 
[assembly: InternalsVisibleTo("Inventory.UnitTests")]
namespace Inventory.Application.Services;

internal class ItemReadService : IItemReadService
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
        _logger.LogInformation("Getting all items");
        var items = await _queryDispatcher
            .DispatchAsync<GetAllItemsQuery, IEnumerable<Item>>(new GetAllItemsQuery());
        return _itemMapper.Map(items);
    }

    /// <inheritdoc />
    public async Task<int> NotifyExpiredItems()
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
}