using Inventory.Domain.Items;

namespace Inventory.Application.Contracts;

public interface IItemWriteService
{
    Task AddItem(Item item);
    Task RemoveItemByName(string name);
}