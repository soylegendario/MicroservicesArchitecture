namespace Inventory.Application.Events;

public class EventPayload<T>
{
    public EventPayload(T @event)
    {
        Event = @event; 
        Timestamp = DateTime.UtcNow;
    }
    
    public T Event { get; }
    
    public DateTime Timestamp { get; }
    
}