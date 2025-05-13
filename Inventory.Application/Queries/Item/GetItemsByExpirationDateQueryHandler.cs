using CrossCutting.Cqrs.Queries;
using Inventory.Domain.Items;
using Microsoft.Extensions.Logging;
using UnitOfWorkinator.Abstractions;

namespace Inventory.Application.Queries.Item;

internal class GetItemsByExpirationDateQueryHandler(
    ILogger<GetItemsByExpirationDateQueryHandler> logger,
    IUnitOfWork unitOfWork)
    : IQueryHandler<GetItemsByExpirationDateQuery, IEnumerable<Domain.Items.Item>>
{
    public Task<IEnumerable<Domain.Items.Item>> HandleAsync(GetItemsByExpirationDateQuery query, CancellationToken cancellation = default)
    {
        logger.LogInformation("GetItemsByExpirationDateQueryHandler: Handling GetItemsByExpirationDateQuery");
        var items = unitOfWork.Repository<IItemRepository>().GetItemsByExpirationDate(query.ExpirationDate);
        return Task.FromResult(items);
    }
}