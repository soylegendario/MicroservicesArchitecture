using Inventory.CrossCutting.Exceptions;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Persistence;

namespace Inventory.Infrastructure.Repository;

public class ItemRepository : IItemRepository
{
    private readonly InventoryDbContext _context;

    public ItemRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public void AddItem(Item item)
    {
        _context.Add(item);
        _context.SaveChanges();
    }

    public IEnumerable<Item> GetAllItems()
    {
        return _context.Items.ToArray();
    }

    public IEnumerable<Item> GetItemsByExpirationDate(DateTime expirationDate)
    {
        return _context.Items.Where(i => i.ExpirationDate.Date == expirationDate.Date);
    }

    public void RemoveItemByName(string name)
    {
        var item = _context.Items.FirstOrDefault(i => i.Name == name);
        if (item is null)
        {
            throw new ItemNotFoundException($"Item with name {name} not found");
        }

        _context.Remove(item);
        _context.SaveChanges();
    }

    public void UpdateItem(Item item)
    {
        _context.Update(item);
        _context.SaveChanges();
    }
}