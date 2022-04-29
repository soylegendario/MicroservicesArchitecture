using Inventory.Domain.Items;

namespace Inventory.Infrastructure.Persistence;

public interface IInventoryContext
{
    IList<Item> Items();
}