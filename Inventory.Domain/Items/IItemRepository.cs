namespace Inventory.Domain.Items;

public interface IItemRepository
{
    void AddItem(Item item);
    
    IEnumerable<Item> GetAllItems();
    
    IEnumerable<Item> GetItemsByExpirationDate(DateTime expirationDate);
    
    void RemoveItemByName(string name);
    
    void UpdateItem(Item item);
}