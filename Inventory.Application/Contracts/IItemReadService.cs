using Inventory.Application.Dto;

namespace Inventory.Application.Contracts;

public interface IItemReadService
{
    /// <summary>
    /// Get all items
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ItemDto>> GetAllItems();
    
    /// <summary>
    /// Notify expired items
    /// </summary>
    /// <returns></returns>
    Task<int> NotifyExpiredItems();
    
    /// <summary>
    /// Get item by ID
    /// </summary>
    /// <param name="id">The ID of the item</param>
    /// <returns>The item with the specified ID</returns>
    Task<ItemDto?> GetItemByIdAsync(Guid id);
}