using Inventory.Application.Dto;

namespace Inventory.Application.Contracts;

public interface IItemWriteService
{
    /// <summary>
    /// Add item to inventory
    /// </summary>
    /// <param name="item">Item to ada</param>
    Task AddItem(ItemDto item);
    
    /// <summary>
    /// Remove item from inventory
    /// </summary>
    /// <param name="name">Name of item to remove</param>
    Task RemoveItemByName(string name);
}