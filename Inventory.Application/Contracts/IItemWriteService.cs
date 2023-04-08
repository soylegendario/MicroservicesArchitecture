using Inventory.Application.Dto;

namespace Inventory.Application.Contracts;

public interface IItemWriteService
{
    /// <summary>
    /// Add item to inventory
    /// </summary>
    /// <param name="item">Item to ada</param>
    Task AddItemAsync(ItemDto item);
    
    /// <summary>
    /// Remove item from inventory
    /// </summary>
    /// <param name="name">Name of item to remove</param>
    Task RemoveItemByNameAsync(string name);

    /// <summary>
    /// Update a item from inventory
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    Task UpdateItemAsync(ItemDto item);
}