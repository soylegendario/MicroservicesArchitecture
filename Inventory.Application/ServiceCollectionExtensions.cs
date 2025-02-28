using CrossCutting.Cqrs.Commands;
using CrossCutting.Cqrs.Queries;
using FluentValidation;
using Inventory.Application.Commands.Item;
using Inventory.Application.Contracts;
using Inventory.Application.Dto;
using Inventory.Application.Mappers.Items;
using Inventory.Application.Queries.Item;
using Inventory.Application.Services;
using Inventory.Application.Validators;
using Inventory.Domain.Items;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IValidator<ItemDto>, ItemValidator>();
        services.AddScoped<IItemMapper, ItemMapper>();
        
        services
            .AddScoped<IItemReadService, ItemReadService>()
            .AddScoped<IItemWriteService, ItemWriteService>();
        
        services
            .AddScoped<IQueryHandler<GetAllItemsQuery, IEnumerable<Item>>, GetAllItemsQueryHandler>()
            .AddScoped<IQueryHandler<GetItemByIdQuery, Item?>, GetItemByIdQueryHandler>()
            .AddScoped<IQueryHandler<GetItemsByExpirationDateQuery, IEnumerable<Item>>, GetItemsByExpirationDateQueryHandler>();
        
        services
            .AddScoped<ICommandHandler<AddItemCommand>, AddItemCommandHandler>()
            .AddScoped<ICommandHandler<RemoveItemByNameCommand>, RemoveItemByNameCommandHandler>()
            .AddScoped<ICommandHandler<UpdateItemCommand>, UpdateItemCommandHandler>();

        return services;
    }
}