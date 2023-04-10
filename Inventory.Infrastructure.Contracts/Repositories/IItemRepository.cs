using Inventory.CrossCutting.Data;
using Inventory.Domain.Items;

namespace Inventory.Infrastructure.Repositories;

public interface IItemRepository : IRepository
{
    void AddItem(Item item);
    
    IEnumerable<Item> GetAllItems();
    
    IEnumerable<Item> GetItemsByExpirationDate(DateTime expirationDate);
    
    void RemoveItemByName(string name);
    
    void UpdateItem(Item item);
}