using Inventory.CrossCutting.Cqrs.Commands;
using Inventory.Domain.Items;
using Microsoft.Extensions.Logging;

namespace Inventory.Infrastructure.Commands;

public class AddItemCommandHandler : ICommandHandler<AddItemCommand>
{
    private readonly ILogger<AddItemCommandHandler> _logger;
    private readonly IItemRepository _itemRepository;

    public AddItemCommandHandler(ILogger<AddItemCommandHandler> logger, IItemRepository itemRepository)
    {
        _logger = logger;
        _itemRepository = itemRepository;
    }

    public Task HandleAsync(AddItemCommand command, CancellationToken cancellation = default)
    {
        try
        {
            _logger.LogInformation("Adding item with Name: {Name} and ExpirationDate {ExpirationDate}",
                command.Item.Name,
                command.Item.ExpirationDate);
            _itemRepository.AddItem(command.Item);
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error adding item");
            throw;
        }
    }
}