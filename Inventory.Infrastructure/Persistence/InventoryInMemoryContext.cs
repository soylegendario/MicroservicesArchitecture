using Inventory.Domain.Items;

namespace Inventory.Infrastructure.Persistence;

public class InventoryInMemoryContext 
{
    public InventoryInMemoryContext()
    {
        Items = new List<Item>();
    }

    public IList<Item> Items { get; }

}