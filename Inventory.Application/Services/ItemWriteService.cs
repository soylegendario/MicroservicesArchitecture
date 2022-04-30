using Inventory.Application.Contracts;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Commands;
using Inventory.Infrastructure.Helpers.Cqrs.Commands;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Services;

public class ItemWriteService : IItemWriteService
{
    private readonly ILogger<ItemWriteService> _logger;
    private readonly ICommandDispatcher _commandDispatcher;

    public ItemWriteService(ILogger<ItemWriteService> logger, ICommandDispatcher commandDispatcher)
    {
        _logger = logger;
        _commandDispatcher = commandDispatcher;
    }

    public Task AddItem(Item item)
    {
        try
        {
            _logger.LogInformation("Adding item");
            var command = new AddItemCommand()
            {
                Item = item
            };
            return _commandDispatcher.DispatchAsync(command);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error adding item");
            throw;
        }
    }

    public Task RemoveItemByName(string name)
    {
        try
        {
            _logger.LogInformation("Removing item by name");
            var command = new RemoveItemByNameCommand
            {
                Name = name
            };
            return _commandDispatcher.DispatchAsync(command);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error removing item by name");
            throw;
        }
    }
}