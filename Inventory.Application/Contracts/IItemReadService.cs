using Inventory.Domain.Items;

namespace Inventory.Application.Contracts;

public interface IItemReadService
{
    Task<IEnumerable<Item>> GetAllItems();
}