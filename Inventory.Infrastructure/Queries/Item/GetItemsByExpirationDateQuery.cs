using Inventory.Infrastructure.Helpers.Cqrs.Queries;

namespace Inventory.Infrastructure.Queries;

public class GetItemsByExpirationDateQuery : IQuery
{
    public DateTime ExpirationDate { get; set; }
}