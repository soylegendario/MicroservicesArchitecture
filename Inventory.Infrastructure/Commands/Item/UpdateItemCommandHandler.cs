using Inventory.CrossCutting.Cqrs.Commands;
using Inventory.CrossCutting.Data;
using Inventory.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace Inventory.Infrastructure.Commands;

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