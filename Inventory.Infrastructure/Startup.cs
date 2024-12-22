using Inventory.Application.Queries;
using Inventory.CrossCutting.Cqrs.Commands;
using Inventory.CrossCutting.Cqrs.Queries;
using Inventory.CrossCutting.Data;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfigurationRoot configurationRoot)
    {
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork<InventoryDbContext>>();

        var connString = configurationRoot.GetConnectionString("InventoryItems");
        services.AddDbContext<InventoryDbContext>(options => 
            options.UseSqlServer(connString));

        return services;
    }
}