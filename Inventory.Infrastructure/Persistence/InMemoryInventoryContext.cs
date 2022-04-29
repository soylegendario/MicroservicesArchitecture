using Inventory.Domain.Items;

namespace Inventory.Infrastructure.Persistence;

public class InMemoryInventoryContext : IInventoryContext
{
    private readonly IList<Item> _items;

    public InMemoryInventoryContext()
    {
        _items = new List<Item>();
    }
    
    public IList<Item> Items()
    {
        return _items;
    }
}