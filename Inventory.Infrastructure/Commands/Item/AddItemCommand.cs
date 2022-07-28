using Inventory.CrossCutting.Cqrs.Commands;
using Inventory.Domain.Items;

namespace Inventory.Infrastructure.Commands;

public class AddItemCommand : ICommand
{
    public Item Item { get; set; }
}