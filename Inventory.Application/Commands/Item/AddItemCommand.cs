using CrossCutting.Cqrs.Commands;

namespace Inventory.Application.Commands.Item;

public class AddItemCommand : ICommand
{
    public Domain.Items.Item Item { get; set; }
}