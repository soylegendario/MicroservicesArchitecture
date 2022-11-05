using System.Runtime.CompilerServices;
using FluentValidation;
using Inventory.Application.Contracts;
using Inventory.Application.Dto;
using Inventory.Application.Mappers.Items;
using Inventory.CrossCutting.Cqrs.Commands;
using Inventory.CrossCutting.Events;
using Inventory.Infrastructure.Commands;
using Inventory.Infrastructure.Events;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")] 
[assembly: InternalsVisibleTo("Inventory.UnitTests")]
namespace Inventory.Application.Services;

internal class ItemWriteService : IItemWriteService
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

    /// <inheritdoc />
    public Task AddItem(ItemDto item)
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
    public async Task RemoveItemByName(string name)
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
}