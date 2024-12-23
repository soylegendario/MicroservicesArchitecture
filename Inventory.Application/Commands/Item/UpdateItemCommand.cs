using CrossCutting.Cqrs.Commands;

namespace Inventory.Application.Commands.Item;

public class UpdateItemCommand : ICommand
{
    public Domain.Items.Item Item { get; set; }

}