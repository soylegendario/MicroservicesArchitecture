using Inventory.CrossCutting.Cqrs.Commands;
using Inventory.Domain.Items;
using Microsoft.Extensions.Logging;

namespace Inventory.Infrastructure.Commands;

internal class RemoveItemByNameCommandHandler : ICommandHandler<RemoveItemByNameCommand>
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
        _logger.LogInformation("Removing item by name: {Name}", command.Name);
        _itemRepository.RemoveItemByName(command.Name);
        return Task.CompletedTask;
    }
}