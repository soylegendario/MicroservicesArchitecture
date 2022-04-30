using Inventory.Application.Contracts;
using Inventory.Domain.Items;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Services;

public class ItemService : IItemService
{
    private readonly ILogger<ItemService> _logger;
    private readonly IItemRepository _itemRepository;

    public ItemService(ILogger<ItemService> logger, IItemRepository itemRepository)
    {
        _logger = logger;
        _itemRepository = itemRepository;
    }

    public void AddItem(Item item)
    {
        try
        {
            _logger.LogInformation("Adding item");
            _itemRepository.AddItem(item);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error adding item");
            throw;
        }
    }

    public IEnumerable<Item> GetAllItems()
    {
        try
        {
            _logger.LogInformation("Getting all items");
            return _itemRepository.GetAllItems();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting all items");
            throw;
        }
    }

    public void RemoveItemByName(string name)
    {
        try
        {
            _logger.LogInformation("Removing item by name");
            _itemRepository.RemoveItemByName(name);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error removing item by name");
            throw;
        }
    }
}