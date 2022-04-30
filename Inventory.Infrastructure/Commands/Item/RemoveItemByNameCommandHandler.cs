using Inventory.Domain.Items;
using Inventory.Infrastructure.Helpers.Cqrs.Commands;
using Microsoft.Extensions.Logging;

namespace Inventory.Infrastructure.Commands;

public class RemoveItemByNameCommandHandler : ICommandHandler<RemoveItemByNameCommand>
{
    private readonly ILogger<RemoveItemByNameCommandHandler> _logger;
    private readonly IItemRepository _itemRepository;

    public RemoveItemByNameCommandHandler(ILogger<RemoveItemByNameCommandHandler> logger, IItemRepository itemRepository)
    {
        _logger = logger;
        _itemRepository = itemRepository;
    }

    public Task HandleAsync(RemoveItemByNameCommand command, CancellationToken cancellation = default)
    {
        try
        {
            _logger.LogInformation("Removing item by name: {Name}", command.Name);
            _itemRepository.RemoveItemByName(command.Name);
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while removing item by name");
            throw;
        }
    }
}