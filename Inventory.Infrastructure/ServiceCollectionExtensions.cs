using CrossCutting.Cqrs.Commands;
using CrossCutting.Cqrs.Queries;
using CrossCutting.Data;
using CrossCutting.Data.SqlServer;
using Inventory.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfigurationRoot configurationRoot)
    {
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        
        services.AddUnitOfWork<InventoryDbContext>(configurationRoot.GetConnectionString("InventoryItems")!);
        services.AddRepositories();

        return services;
    }
}