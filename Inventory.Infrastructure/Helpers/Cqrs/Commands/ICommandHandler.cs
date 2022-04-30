namespace Inventory.Infrastructure.Helpers.Cqrs.Commands;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task HandleAsync(TCommand command, CancellationToken cancellation = default);
}