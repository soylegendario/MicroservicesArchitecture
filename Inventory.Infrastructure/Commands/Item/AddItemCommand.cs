using Inventory.Domain.Items;
using Inventory.Infrastructure.Helpers.Cqrs.Commands;

namespace Inventory.Infrastructure.Commands;

public class AddItemCommand : ICommand
{
    public Item Item { get; set; }
}