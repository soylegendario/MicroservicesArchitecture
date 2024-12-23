using CrossCutting.Cqrs.Commands;
using CrossCutting.Data;
using Inventory.Domain.Items;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Commands.Item;

internal class AddItemCommandHandler(ILogger<AddItemCommandHandler> logger, IUnitOfWork unitOfWork)
    : ICommandHandler<AddItemCommand>
{
    public Task HandleAsync(AddItemCommand command, CancellationToken cancellation = default)
    {
        logger.LogInformation("Adding item with Name: {Name} and ExpirationDate {ExpirationDate}",
            command.Item.Name,
            command.Item.ExpirationDate);
        unitOfWork.Repository<IItemRepository>().AddItem(command.Item);
        return unitOfWork.SaveChangesAsync();
    }
}