using CrossCutting.Cqrs.Commands;
using CrossCutting.Data;
using Inventory.Domain.Items;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Commands.Item;

internal class RemoveItemByNameCommandHandler(ILogger<RemoveItemByNameCommandHandler> logger, IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveItemByNameCommand>
{
    public Task HandleAsync(RemoveItemByNameCommand command, CancellationToken cancellation = default)
    {
        logger.LogInformation("Removing item by name: {Name}", command.Name);
        unitOfWork.Repository<IItemRepository>().RemoveItemByName(command.Name);
        return unitOfWork.SaveChangesAsync();
    }
}