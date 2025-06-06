﻿using Inventory.Application.Exceptions;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using UnitOfWorkinator.EFCore;

namespace Inventory.Infrastructure.Repositories;

public class ItemRepository(DbContext context) : BaseRepository(context), IItemRepository
{
    private DbSet<Item> Items => Context.Set<Item>();
    
    public void AddItem(Item item)
    {
        Context.Add(item);
    }

    public IEnumerable<Item> GetAllItems()
    {
        return [.. Items];
    }

    public IEnumerable<Item> GetItemsByExpirationDate(DateTime expirationDate)
    {
        return Items.Where(i => i.ExpirationDate.Date == expirationDate.Date);
    }

    public void RemoveItemByName(string name)
    {
        var item = Items.FirstOrDefault(i => i.Name == name);
        if (item is null)
        {
            throw new ItemNotFoundException($"Item with name {name} not found");
        }

        Context.Remove(item);
    }

    public void UpdateItem(Item item)
    {
        Context.Update(item);
    }
    
    public async Task<Item> GetItemByIdAsync(Guid id)
    {
        var item = await Items.FirstOrDefaultAsync(i => i.Id == id);
        if (item == null)
        {
            throw new ItemNotFoundException($"Item with ID {id} not found");
        }
        return item;
    }
}