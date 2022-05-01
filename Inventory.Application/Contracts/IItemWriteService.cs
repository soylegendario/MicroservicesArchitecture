using Inventory.Application.Dto;

namespace Inventory.Application.Contracts;

public interface IItemWriteService
{
    Task AddItem(ItemDto item);
    Task RemoveItemByName(string name);
}