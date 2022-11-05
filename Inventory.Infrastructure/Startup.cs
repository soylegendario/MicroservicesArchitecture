using Inventory.CrossCutting.Cqrs.Commands;
using Inventory.CrossCutting.Cqrs.Queries;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Commands;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Queries;
using Inventory.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.AddTransient<IQueryHandler<GetAllItemsQuery, IEnumerable<Item>>, GetAllItemsQueryHandler>();
        services.AddTransient<IQueryHandler<GetItemsByExpirationDateQuery, IEnumerable<Item>>, GetItemsByExpirationDateQueryHandler>();
        services.AddTransient<ICommandHandler<AddItemCommand>, AddItemCommandHandler>();
        services.AddTransient<ICommandHandler<RemoveItemByNameCommand>, RemoveItemByNameCommandHandler>();
        services.AddSingleton<InventoryInMemoryContext>();
        services.AddTransient<IItemRepository, ItemInMemoryRepository>();

        return services;
    }
}