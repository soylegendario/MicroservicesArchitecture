using Microsoft.Extensions.DependencyInjection;

namespace CrossCutting.Cqrs.Queries;

public class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    public Task<TQueryResult> DispatchAsync<TQuery, TQueryResult>(TQuery query,
        CancellationToken cancellation = default)
        where TQuery : IQuery
    {
        var handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TQueryResult>>();
        return handler.HandleAsync(query, cancellation);
    }
}