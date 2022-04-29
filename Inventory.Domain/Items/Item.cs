namespace Inventory.Domain.Items;

public class Item
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public DateTime ExpirationDate { get; set; }
}