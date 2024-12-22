using FluentValidation;
using Inventory.Application.Commands;
using Inventory.Application.Contracts;
using Inventory.Application.Dto;
using Inventory.Application.Events;
using Inventory.Application.Mappers.Items;
using Inventory.CrossCutting.Cqrs.Commands;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Services;

internal class ItemWriteService : IItemWriteService
{
    private readonly ILogger<ItemWriteService> _logger;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IItemMapper _itemMapper;
    private readonly IEventBus _eventBus;
    private readonly IValidator<ItemDto> _validator;

    public ItemWriteService(ILogger<ItemWriteService> logger,
        ICommandDispatcher commandDispatcher,
        IItemMapper itemMapper,
        IEventBus eventBus,
        IValidator<ItemDto> validator)
    {
        _logger = logger;
        _commandDispatcher = commandDispatcher;
        _itemMapper = itemMapper;
        _eventBus = eventBus;
        _validator = validator;
    }

    /// <inheritdoc />
    public Task AddItemAsync(ItemDto item)
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

    /// <inheritdoc />
    public async Task RemoveItemByNameAsync(string name)
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

    /// <inheritdoc />
    public async Task UpdateItemAsync(ItemDto item)
    {
        _logger.LogInformation("Updating item");
        var command = new UpdateItemCommand()
        {
            Item = _itemMapper.Map(item)
        };
        await _commandDispatcher.DispatchAsync(command);
    }
}