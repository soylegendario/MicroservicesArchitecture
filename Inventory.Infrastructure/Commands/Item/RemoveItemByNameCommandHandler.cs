using Inventory.CrossCutting.Cqrs.Commands;
using Inventory.CrossCutting.Data;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace Inventory.Infrastructure.Commands;

internal class RemoveItemByNameCommandHandler : ICommandHandler<RemoveItemByNameCommand>
{
    private readonly ILogger<RemoveItemByNameCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveItemByNameCommandHandler(ILogger<RemoveItemByNameCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public Task HandleAsync(RemoveItemByNameCommand command, CancellationToken cancellation = default)
    {
        _logger.LogInformation("Removing item by name: {Name}", command.Name);
        _unitOfWork.Repository<IItemRepository>().RemoveItemByName(command.Name);
        return _unitOfWork.SaveChangesAsync();
    }
}