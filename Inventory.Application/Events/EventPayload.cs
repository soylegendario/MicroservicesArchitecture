namespace Inventory.Application.Events;

public class EventPayload<T>(T @event)
{
    public T Event { get; } = @event;

    public DateTime Timestamp { get; } = DateTime.UtcNow;
}