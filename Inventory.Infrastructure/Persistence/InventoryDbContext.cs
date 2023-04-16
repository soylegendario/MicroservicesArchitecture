using Inventory.Domain.Items;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence;

public sealed class InventoryDbContext : DbContext
{
    public DbSet<Item> Items { get; set; } = null!;

    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
    }
}