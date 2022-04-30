using Inventory.Domain.Items;
using Inventory.Infrastructure.Exceptions;
using Inventory.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace Inventory.Infrastructure.Repository;

public class ItemRepository : IItemRepository
{
    private readonly ILogger<ItemRepository> _logger;
    private readonly IInventoryContext _inventoryContext;

    public ItemRepository(ILogger<ItemRepository> logger, IInventoryContext inventoryContext)
    {
        _logger = logger;
        _inventoryContext = inventoryContext;
    }

    public void AddItem(Item item)
    {
        try
        {
            _logger.LogInformation("Adding item to database");
            item.Id = Guid.NewGuid();
            _inventoryContext.Items().Add(item);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error adding item to database");
            throw;
        }
    }

    public IEnumerable<Item> GetAllItems()
    {
        try
        {
            return _inventoryContext.Items();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting all items from database");
            throw;
        }
    }

    public void RemoveItemByName(string name)
    {
        try
        {
            _logger.LogInformation("Removing item with name {Name} from database", name);
            var item = _inventoryContext.Items().FirstOrDefault(i => i.Name == name);
            if (item is null)
            {
                throw new ItemNotFoundException($"Item with name {name} not found");
            }

            _inventoryContext.Items().Remove(item);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error removing item from database");
            throw;
        }
    }
}