using FluentValidation;
using Inventory.Application.Contracts;
using Inventory.Application.Dto;
using Inventory.Application.Mappers.Items;
using Inventory.Infrastructure.Commands;
using Inventory.Infrastructure.Events;
using Inventory.Infrastructure.Helpers.Cqrs.Commands;
using Inventory.Infrastructure.Helpers.Events;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Services;

public class ItemWriteService : IItemWriteService
{
    private readonly ILogger<ItemWriteService> _logger;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IItemMapper _itemMapper;
    private readonly IEventBus _eventBus;
    private readonly AbstractValidator<ItemDto> _validator;

    public ItemWriteService(ILogger<ItemWriteService> logger,
        ICommandDispatcher commandDispatcher,
        IItemMapper itemMapper,
        IEventBus eventBus,
        AbstractValidator<ItemDto> validator)
    {
        _logger = logger;
        _commandDispatcher = commandDispatcher;
        _itemMapper = itemMapper;
        _eventBus = eventBus;
        _validator = validator;
    }

    public Task AddItem(ItemDto item)
    {
        try
        {
            _logger.LogInformation("Adding item");
            var validationResult = _validator.Validate(item);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var command = new AddItemCommand()
            {
                Item = _itemMapper.Map(item)
            };
            return _commandDispatcher.DispatchAsync(command);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error adding item");
            throw;
        }
    }

    public async Task RemoveItemByName(string name)
    {
        try
        {
            _logger.LogInformation("Removing item by name");
            var command = new RemoveItemByNameCommand
            {
                Name = name
            };
            await _commandDispatcher.DispatchAsync(command);
            _eventBus.Publish(new ItemRemovedEvent
            {
                Name = name
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error removing item by name");
            throw;
        }
    }
}