using Inventory.Domain.Items;
using Inventory.Infrastructure.Persistence;

namespace Inventory.Infrastructure.Repository;

public class ItemRepository : IItemRepository
{
    private readonly IInventoryContext _inventoryContext;

    public ItemRepository(IInventoryContext inventoryContext)
    {
        _inventoryContext = inventoryContext;
    }

    public void AddItem(Item item)
    {
        item.Id = Guid.NewGuid();
        _inventoryContext.Items().Add(item);
    }

    public IEnumerable<Item> GetAllItems()
    {
        return _inventoryContext.Items();
    }

    public void RemoveItemByName(string name)
    {
        var item = _inventoryContext.Items().FirstOrDefault(i => i.Name == name);
        if (item != null)
        {
            _inventoryContext.Items().Remove(item);
        }
    }
}