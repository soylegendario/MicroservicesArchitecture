namespace Inventory.Application.Dto;

public class ItemDto
{
    public Guid? Id { get; set; }
    
    public string Name { get; set; }

    public DateTime ExpirationDate { get; set; }
}