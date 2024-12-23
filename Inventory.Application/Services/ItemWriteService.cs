using CrossCutting.Cqrs.Commands;
using FluentValidation;
using Inventory.Application.Commands.Item;
using Inventory.Application.Contracts;
using Inventory.Application.Dto;
using Inventory.Application.Events;
using Inventory.Application.Mappers.Items;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Services;

internal class ItemWriteService(
    ILogger<ItemWriteService> logger,
    ICommandDispatcher commandDispatcher,
    IItemMapper itemMapper,
    IEventBus eventBus,
    IValidator<ItemDto> validator)
    : IItemWriteService
{
    /// <inheritdoc />
    public Task AddItemAsync(ItemDto item)
    {
        logger.LogInformation("Adding item");
        var validationResult = validator.Validate(item);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        var command = new AddItemCommand()
        {
            Item = itemMapper.Map(item)
        };
        return commandDispatcher.DispatchAsync(command);
    }

    /// <inheritdoc />
    public async Task RemoveItemByNameAsync(string name)
    {
        logger.LogInformation("Removing item by name");
        var command = new RemoveItemByNameCommand
        {
            Name = name
        };
        await commandDispatcher.DispatchAsync(command);
        eventBus.Publish(new ItemRemovedEvent
        {
            Name = name
        });
    }

    /// <inheritdoc />
    public async Task UpdateItemAsync(ItemDto item)
    {
        logger.LogInformation("Updating item");
        var command = new UpdateItemCommand()
        {
            Item = itemMapper.Map(item)
        };
        await commandDispatcher.DispatchAsync(command);
    }
}