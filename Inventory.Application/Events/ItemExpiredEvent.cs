namespace Inventory.Application.Events;

public class ItemExpiredEvent
{
    public string Name { get; set; }
    public DateTime ExpirationDate { get; set; }
}