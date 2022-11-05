using Inventory.CrossCutting.Cqrs.Queries;
using Inventory.Domain.Items;
using Microsoft.Extensions.Logging;

namespace Inventory.Infrastructure.Queries;

internal class GetItemsByExpirationDateQueryHandler : IQueryHandler<GetItemsByExpirationDateQuery, IEnumerable<Item>>
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
        _logger.LogInformation("GetItemsByExpirationDateQueryHandler: Handling GetItemsByExpirationDateQuery");
        var items = _itemRepository.GetItemsByExpirationDate(query.ExpirationDate);
        return Task.FromResult(items);
    }
}