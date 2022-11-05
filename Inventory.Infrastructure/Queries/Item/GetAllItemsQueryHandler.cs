using Inventory.CrossCutting.Cqrs.Queries;
using Inventory.Domain.Items;
using Microsoft.Extensions.Logging;

namespace Inventory.Infrastructure.Queries;

internal class GetAllItemsQueryHandler : IQueryHandler<GetAllItemsQuery, IEnumerable<Item>>
{
    private readonly ILogger<GetAllItemsQueryHandler> _logger;
    private readonly IItemRepository _itemRepository;


    public GetAllItemsQueryHandler(ILogger<GetAllItemsQueryHandler> logger, IItemRepository itemRepository)
    {
        _logger = logger;
        _itemRepository = itemRepository;
    }

    public Task<IEnumerable<Item>> HandleAsync(GetAllItemsQuery query, CancellationToken cancellation = default)
    {
        _logger.LogInformation("GetAllItemsQueryHandler: Handling GetAllItemsQuery");
        return Task.FromResult(_itemRepository.GetAllItems());
    }
}