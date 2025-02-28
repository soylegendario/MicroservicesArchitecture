using CrossCutting.Cqrs.Queries;
using CrossCutting.Data;
using Inventory.Domain.Items;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Queries.Item;

internal class GetItemByIdQueryHandler(
    ILogger<GetItemByIdQueryHandler> logger,
    IUnitOfWork unitOfWork)
    : IQueryHandler<GetItemByIdQuery, Domain.Items.Item?>
{
    public async Task<Domain.Items.Item?> HandleAsync(GetItemByIdQuery query, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("GetItemByIdQueryHandler: Handling GetItemByIdQuery for ID: {Id}", query.Id);
        var item = await unitOfWork.Repository<IItemRepository>().GetItemByIdAsync(query.Id);
        return item;
    }
}