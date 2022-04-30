using Inventory.Domain.Items;

namespace Inventory.Application.Contracts;

public interface IItemService
{
    void AddItem(Item item);
    IEnumerable<Item> GetAllItems();
    void RemoveItemByName(string name);
}