using CrossCutting.Cqrs.Commands;
using CrossCutting.Data;
using Inventory.Domain.Items;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Commands;

public class UpdateItemCommandHandler : ICommandHandler<UpdateItemCommand>
{
    private readonly ILogger<UpdateItemCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateItemCommandHandler(ILogger<UpdateItemCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public Task HandleAsync(UpdateItemCommand command, CancellationToken cancellation = default)
    {
        _logger.LogInformation("Update item {Id} with Name: {Name} and ExpirationDate {ExpirationDate}",
            command.Item.Id,
            command.Item.Name,
            command.Item.ExpirationDate);
        _unitOfWork.Repository<IItemRepository>().UpdateItem(command.Item);
        return _unitOfWork.SaveChangesAsync();
    }
}