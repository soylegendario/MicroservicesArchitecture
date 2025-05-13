using CrossCutting.Cqrs.Queries;
using Inventory.Domain.Items;
using Microsoft.Extensions.Logging;
using UnitOfWorkinator.Abstractions;

namespace Inventory.Application.Queries.Item;

internal class GetAllItemsQueryHandler(ILogger<GetAllItemsQueryHandler> logger, IUnitOfWork unitOfWork)
    : IQueryHandler<GetAllItemsQuery, IEnumerable<Domain.Items.Item>>
{
    public Task<IEnumerable<Domain.Items.Item>> HandleAsync(GetAllItemsQuery query, CancellationToken cancellation = default)
    {
        logger.LogInformation("GetAllItemsQueryHandler: Handling GetAllItemsQuery");
        return Task.FromResult(unitOfWork.Repository<IItemRepository>().GetAllItems());
    }
}