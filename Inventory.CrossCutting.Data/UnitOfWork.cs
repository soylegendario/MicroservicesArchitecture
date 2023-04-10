using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.CrossCutting.Data;

/// <inheritdoc />
public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly object _lock = new();
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
        if (_repositories.TryGetValue(typeof(T), out var repository))
        {
            return (T)repository;
        }

        lock (_lock)
        {
            repository = _serviceProvider.GetRequiredService<T>();
            repository.SetContext(_context);
            _repositories.TryAdd(typeof(T), repository);
            return (T)repository;
        }
    }

    /// <inheritdoc />
    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}