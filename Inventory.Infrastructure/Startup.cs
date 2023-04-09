using Inventory.CrossCutting.Cqrs.Commands;
using Inventory.CrossCutting.Cqrs.Queries;
using Inventory.CrossCutting.Data;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Commands;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Queries;
using Inventory.Infrastructure.Repository;
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
        services.AddScoped<IQueryHandler<GetAllItemsQuery, IEnumerable<Item>>, GetAllItemsQueryHandler>();
        services.AddScoped<IQueryHandler<GetItemsByExpirationDateQuery, IEnumerable<Item>>, GetItemsByExpirationDateQueryHandler>();
        services.AddScoped<ICommandHandler<AddItemCommand>, AddItemCommandHandler>();
        services.AddScoped<ICommandHandler<RemoveItemByNameCommand>, RemoveItemByNameCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateItemCommand>, UpdateItemCommandHandler>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork<InventoryDbContext>>();

        var connString = configurationRoot.GetConnectionString("InventoryItems");
        services.AddDbContext<InventoryDbContext>(options => 
            options.UseSqlServer(connString));

        return services;
    }
}