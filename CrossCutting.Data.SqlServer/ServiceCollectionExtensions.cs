using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CrossCutting.Data.SqlServer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUnitOfWork<T>(this IServiceCollection services, string connectionString) 
        where T : DbContext
    {
        services.AddDbContext<T>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IUnitOfWork, UnitOfWork<T>>();
        return services;
    }
    

}