using CrossCutting.Cqrs.Commands;

namespace Inventory.Application.Commands.Item;

public class RemoveItemByNameCommand : ICommand
{
    public string Name { get; set; }
}