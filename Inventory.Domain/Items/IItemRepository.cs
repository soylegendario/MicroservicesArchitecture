namespace Inventory.Domain.Items;

public interface IItemRepository
{
    void AddItem(Item item);
    IEnumerable<Item> GetAllItems();
    void RemoveItemByName(string name);
}