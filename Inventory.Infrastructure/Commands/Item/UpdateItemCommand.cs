using Inventory.CrossCutting.Cqrs.Commands;
using Inventory.Domain.Items;

namespace Inventory.Infrastructure.Commands;

public class UpdateItemCommand : ICommand
{
    public Item Item { get; set; }

}