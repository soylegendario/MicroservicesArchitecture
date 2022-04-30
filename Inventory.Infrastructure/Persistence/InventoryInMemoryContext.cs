using Inventory.Domain.Items;

namespace Inventory.Infrastructure.Persistence;

public class InventoryInMemoryContext 
{
    public InventoryInMemoryContext()
    {
        Items = new List<Item>();
    }

    public virtual IList<Item> Items { get; }

}