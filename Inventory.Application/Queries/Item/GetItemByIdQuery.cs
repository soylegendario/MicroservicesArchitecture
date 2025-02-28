using CrossCutting.Cqrs.Queries;

namespace Inventory.Application.Queries.Item;

public class GetItemByIdQuery : IQuery
{
    public Guid Id { get; set; }
}

