using Inventory.Domain.Items;

namespace Inventory.Application.Dto;

public class ItemDto
{
    public Guid? Id { get; set; }
    
    public string Name { get; set; }

    public DateTime ExpirationDate { get; set; }
    
    public static explicit operator ItemDto(Item item)
    {
        return new ItemDto
        {
            Id = item.Id,
            Name = item.Name,
            ExpirationDate = item.ExpirationDate
        };
    }
    
    public static explicit operator Item(ItemDto item)
    {
        return new Item
        {
            Id = item.Id ?? Guid.NewGuid(),
            Name = item.Name,
            ExpirationDate = item.ExpirationDate

        };
    }
}