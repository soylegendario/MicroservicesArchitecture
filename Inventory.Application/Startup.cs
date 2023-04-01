using FluentValidation;
using Inventory.Application.Contracts;
using Inventory.Application.Dto;
using Inventory.Application.Mappers.Items;
using Inventory.Application.Services;
using Inventory.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Application;

public static class Startup
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IValidator<ItemDto>, ItemValidator>();
        services.AddScoped<IItemMapper, ItemMapper>();
        services.AddScoped<IItemReadService, ItemReadService>();
        services.AddScoped<IItemWriteService, ItemWriteService>();

        return services;
    }
}