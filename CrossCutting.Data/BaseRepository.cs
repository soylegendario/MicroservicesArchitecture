using Microsoft.EntityFrameworkCore;

namespace CrossCutting.Data;

public abstract class BaseRepository(DbContext context): IRepository
{
    protected DbContext Context = context;
}