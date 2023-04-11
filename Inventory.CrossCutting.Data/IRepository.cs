using Microsoft.EntityFrameworkCore;

namespace Inventory.CrossCutting.Data;

public interface IRepository
{
    void SetContext(DbContext context);
}