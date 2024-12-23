using Microsoft.EntityFrameworkCore;

namespace CrossCutting.Data;

public interface IRepository
{
    void SetContext(DbContext context);
}