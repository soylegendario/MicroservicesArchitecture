using Inventory.CrossCutting.Cqrs.Commands;
using Inventory.Domain.Items;
using Microsoft.Extensions.Logging;

namespace Inventory.Infrastructure.Commands;

public class UpdateItemCommandHandler : ICommandHandler<UpdateItemCommand>
{
    private readonly ILogger<UpdateItemCommandHandler> _logger;
    private readonly IItemRepository _itemRepository;

    public UpdateItemCommandHandler(ILogger<UpdateItemCommandHandler> logger, IItemRepository itemRepository)
    {
        _logger = logger;
        _itemRepository = itemRepository;
    }

    public Task HandleAsync(UpdateItemCommand command, CancellationToken cancellation = default)
    {
        _logger.LogInformation("Update item {Id} with Name: {Name} and ExpirationDate {ExpirationDate}",
            command.Item.Id,
            command.Item.Name,
            command.Item.ExpirationDate);
        _itemRepository.UpdateItem(command.Item);
        return Task.CompletedTask;
    }
}