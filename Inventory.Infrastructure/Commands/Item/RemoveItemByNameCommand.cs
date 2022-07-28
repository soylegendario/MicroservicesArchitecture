using Inventory.CrossCutting.Cqrs.Commands;

namespace Inventory.Infrastructure.Commands;

public class RemoveItemByNameCommand : ICommand
{
    public string Name { get; set; }
}