using CrossCutting.Cqrs.Commands;
using CrossCutting.Cqrs.Queries;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UnitOfWorkinator.EFCore;

namespace Inventory.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfigurationRoot configurationRoot)
    {
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        
        services.AddDbContext<InventoryDbContext>(options => options.UseSqlServer(configurationRoot.GetConnectionString("InventoryItems")));
        services.AddUnitOfWorkinator<InventoryDbContext>();

        return services;
    }
}