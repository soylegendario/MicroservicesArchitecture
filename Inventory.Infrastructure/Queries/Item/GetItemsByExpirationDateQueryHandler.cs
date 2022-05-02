using Inventory.Domain.Items;
using Inventory.Infrastructure.Helpers.Cqrs.Queries;
using Microsoft.Extensions.Logging;

namespace Inventory.Infrastructure.Queries;

public class GetItemsByExpirationDateQueryHandler : IQueryHandler<GetItemsByExpirationDateQuery, IEnumerable<Item>>
{
    private readonly ILogger<GetItemsByExpirationDateQueryHandler> _logger;
    private readonly IItemRepository _itemRepository;

    public GetItemsByExpirationDateQueryHandler(ILogger<GetItemsByExpirationDateQueryHandler> logger, IItemRepository itemRepository)
    {
        _logger = logger;
        _itemRepository = itemRepository;
    }

    public Task<IEnumerable<Item>> HandleAsync(GetItemsByExpirationDateQuery query, CancellationToken cancellation = default)
    {
        try
        {
            _logger.LogInformation("GetItemsByExpirationDateQueryHandler: Handling GetItemsByExpirationDateQuery");
            var items = _itemRepository.GetItemsByExpirationDate(query.ExpirationDate);
            return Task.FromResult(items);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "GetItemsByExpirationDateQueryHandler: Error handling GetItemsByExpirationDateQuery");
            throw;
        }
    }
}