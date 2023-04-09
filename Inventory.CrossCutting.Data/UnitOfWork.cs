using Microsoft.EntityFrameworkCore;

namespace Inventory.CrossCutting.Data;

/// <inheritdoc />
public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private static readonly object Lock = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly DbContext _context;

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
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}