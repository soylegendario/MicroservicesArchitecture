using CrossCutting.Cqrs.Queries;
using CrossCutting.Data;
using Inventory.Domain.Items;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Queries.Item;

internal class GetItemsByExpirationDateQueryHandler : IQueryHandler<GetItemsByExpirationDateQuery, IEnumerable<Domain.Items.Item>>
{
    private readonly ILogger<GetItemsByExpirationDateQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetItemsByExpirationDateQueryHandler(ILogger<GetItemsByExpirationDateQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public Task<IEnumerable<Domain.Items.Item>> HandleAsync(GetItemsByExpirationDateQuery query, CancellationToken cancellation = default)
    {
        _logger.LogInformation("GetItemsByExpirationDateQueryHandler: Handling GetItemsByExpirationDateQuery");
        var items = _unitOfWork.Repository<IItemRepository>().GetItemsByExpirationDate(query.ExpirationDate);
        return Task.FromResult(items);
    }
}