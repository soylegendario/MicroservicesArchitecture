namespace Inventory.CrossCutting.Cqrs.Commands;

public interface ICommandDispatcher
{
    Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellation = default) where TCommand : ICommand;
}