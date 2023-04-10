using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.CrossCutting.Data;

/// <inheritdoc />
public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DbContext _context;
    private readonly ConcurrentDictionary<Type, IRepository> _repositories = new();

    /// <summary>
    /// Initializes a new instance of <see><cref>UnitOfWork</cref></see>
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="context"></param>
    public UnitOfWork(IServiceProvider serviceProvider, TContext context)
    {
        _serviceProvider = serviceProvider;
        _context = context;
    }

    /// <inheritdoc />
    public T Repository<T>() where T : IRepository
    {
        return (T)_repositories.GetOrAdd(typeof(T), type => _serviceProvider.GetRequiredService<T>());
    }

    /// <inheritdoc />
    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}