namespace CrossCutting.Cqrs.Queries;

public interface IQueryHandler<in TQuery, TQueryResult> where TQuery : IQuery
{
    Task<TQueryResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}