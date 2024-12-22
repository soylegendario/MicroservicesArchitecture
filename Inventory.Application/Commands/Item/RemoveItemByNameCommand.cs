using Inventory.CrossCutting.Cqrs.Commands;

namespace Inventory.Application.Commands;

public class RemoveItemByNameCommand : ICommand
{
    public string Name { get; set; }
}