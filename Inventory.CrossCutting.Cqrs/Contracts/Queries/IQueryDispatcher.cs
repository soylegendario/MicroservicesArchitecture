namespace Inventory.CrossCutting.Cqrs.Queries;

public interface IQueryDispatcher
{
    Task<TQueryResult> DispatchAsync<TQuery, TQueryResult>(TQuery query, CancellationToken cancellation = default)
        where TQuery : IQuery;
}