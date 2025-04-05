using Microsoft.Extensions.DependencyInjection;

namespace CrossCutting.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, bool registerRepositoriesInServiceCollection = false)
    {
        var repositoryMap = BuildRepositoryMap(services, registerRepositoriesInServiceCollection);
        services.AddSingleton(repositoryMap);
        return services;
    }
    
    private static RepositoryMap BuildRepositoryMap(IServiceCollection services, bool registerRepositoriesInServiceCollection = false)
    {
        var baseInterface = typeof(IRepository);
        var result = new Dictionary<Type, Type>();

        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && a.GetName().Name != null);

        var types = assemblies.SelectMany(a => a.GetTypes())
            .Where(type => 
                type is { IsClass: true, IsAbstract: false } && 
                baseInterface.IsAssignableFrom(type));
        
        foreach (var type in types)
        {
            // Find the main interface that it implements in addition to IRepository
            var repoInterface = type.GetInterfaces()
                .FirstOrDefault(i => 
                    i != baseInterface &&
                    baseInterface.IsAssignableFrom(i) &&
                    i != typeof(IDisposable)); 

            if (repoInterface != null)
            {
                result[repoInterface] = type;
            }
        }

        if (!registerRepositoriesInServiceCollection)
        {
            return new RepositoryMap(result);
        }

        foreach (var (interfaceType, implementationType) in result)
        {
            services.AddScoped(interfaceType, implementationType);
        }

        return new RepositoryMap(result);
    }

}