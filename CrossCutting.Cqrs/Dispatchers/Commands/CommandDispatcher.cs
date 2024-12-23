using Microsoft.Extensions.DependencyInjection;

namespace CrossCutting.Cqrs.Commands;

public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellation = default)
        where TCommand : ICommand
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        return handler.HandleAsync(command, cancellation);
    }
}