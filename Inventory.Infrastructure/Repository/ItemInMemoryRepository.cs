using Inventory.CrossCutting.Exceptions;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace Inventory.Infrastructure.Repository;

internal class ItemInMemoryRepository : IItemRepository
{
    private readonly ILogger<ItemInMemoryRepository> _logger;
    private readonly InventoryInMemoryContext _context;

    public ItemInMemoryRepository(ILogger<ItemInMemoryRepository> logger, InventoryInMemoryContext context)
    {
        _logger = logger;
        _context = context;
    }

    public void AddItem(Item item)
    {
        _logger.LogInformation("Adding item to database");
        item.Id = Guid.NewGuid();
        _context.Items.Add(item);
    }

    public IEnumerable<Item> GetAllItems()
    {
        return _context.Items;
    }

    public IEnumerable<Item> GetItemsByExpirationDate(DateTime expirationDate)
    {
        return _context.Items.Where(i => i.ExpirationDate.Date == expirationDate.Date);
    }

    public void RemoveItemByName(string name)
    {
        _logger.LogInformation("Removing item with name {Name} from database", name);
        var item = _context.Items.FirstOrDefault(i => i.Name == name);
        if (item is null)
        {
            throw new ItemNotFoundException($"Item with name {name} not found");
        }

        _context.Items.Remove(item);
    }
}