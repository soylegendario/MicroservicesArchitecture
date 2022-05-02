using Inventory.Application.Dto;

namespace Inventory.Application.Contracts;

public interface IItemReadService
{
    Task<IEnumerable<ItemDto>> GetAllItems();
    
    Task<int> NotifyExpiredItems();
}