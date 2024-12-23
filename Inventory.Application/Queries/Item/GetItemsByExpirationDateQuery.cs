using CrossCutting.Cqrs.Queries;

namespace Inventory.Application.Queries.Item;

public class GetItemsByExpirationDateQuery : IQuery
{
    public DateTime ExpirationDate { get; set; }
}