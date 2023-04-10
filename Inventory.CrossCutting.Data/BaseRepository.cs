using Microsoft.EntityFrameworkCore;

namespace Inventory.CrossCutting.Data;

public abstract class BaseRepository : IRepository
{
    protected DbContext Context { get; set; } = null!;

    public void SetContext(DbContext context)
    {
        Context = context;
    }
}