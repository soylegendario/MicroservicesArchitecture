using Microsoft.EntityFrameworkCore;

namespace CrossCutting.Data;

public abstract class BaseRepository : IRepository
{
    protected DbContext Context { get; set; } = null!;

    public void SetContext(DbContext context)
    {
        Context = context;
    }
}