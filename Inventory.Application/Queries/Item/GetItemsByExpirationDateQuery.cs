using Inventory.CrossCutting.Cqrs.Queries;

namespace Inventory.Application.Queries;

public class GetItemsByExpirationDateQuery : IQuery
{
    public DateTime ExpirationDate { get; set; }
}