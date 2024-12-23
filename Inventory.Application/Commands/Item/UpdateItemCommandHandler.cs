using CrossCutting.Cqrs.Commands;
using CrossCutting.Data;
using Inventory.Domain.Items;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Commands.Item;

public class UpdateItemCommandHandler(ILogger<UpdateItemCommandHandler> logger, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateItemCommand>
{
    public Task HandleAsync(UpdateItemCommand command, CancellationToken cancellation = default)
    {
        logger.LogInformation("Update item {Id} with Name: {Name} and ExpirationDate {ExpirationDate}",
            command.Item.Id,
            command.Item.Name,
            command.Item.ExpirationDate);
        unitOfWork.Repository<IItemRepository>().UpdateItem(command.Item);
        return unitOfWork.SaveChangesAsync();
    }
}