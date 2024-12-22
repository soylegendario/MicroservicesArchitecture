using Inventory.CrossCutting.Cqrs.Queries;
using Inventory.CrossCutting.Data;
using Inventory.Domain.Items;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Queries;

internal class GetAllItemsQueryHandler : IQueryHandler<GetAllItemsQuery, IEnumerable<Item>>
{
    private readonly ILogger<GetAllItemsQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetAllItemsQueryHandler(ILogger<GetAllItemsQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public Task<IEnumerable<Item>> HandleAsync(GetAllItemsQuery query, CancellationToken cancellation = default)
    {
        _logger.LogInformation("GetAllItemsQueryHandler: Handling GetAllItemsQuery");
        return Task.FromResult(_unitOfWork.Repository<IItemRepository>().GetAllItems());
    }
}