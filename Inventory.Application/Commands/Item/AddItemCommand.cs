using CrossCutting.Cqrs.Commands;
using Inventory.Domain.Items;

namespace Inventory.Application.Commands;

public class AddItemCommand : ICommand
{
    public Item Item { get; set; }
}