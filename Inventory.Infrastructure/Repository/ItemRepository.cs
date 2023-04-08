using Inventory.CrossCutting.Exceptions;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
        // Option 1
        var entity = _context.Entry(item);
        entity.State = EntityState.Modified;
        
        // // Option 2
        // _context.Update(item);
        //
        // // Option 3
        // var entity2 = _context.Items.Find(item.Id);
        // if (entity2 == null)
        // {
        //     throw new ItemNotFoundException($"Item {item.Id} not found");
        // }
        // entity2.Name = item.Name;
        // entity2.ExpirationDate = item.ExpirationDate;
        
        _context.SaveChanges();
    }
}