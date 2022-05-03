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
}