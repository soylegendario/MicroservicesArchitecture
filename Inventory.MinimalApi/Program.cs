using System.Net;
using Inventory.Application.Contracts;
using Inventory.Application.Dto;
using Inventory.Application.Mappers.Items;
using Inventory.Application.Services;
using Inventory.Domain.Items;
using Inventory.Infrastructure.Commands;
using Inventory.Infrastructure.Events;
using Inventory.Infrastructure.Exceptions;
using Inventory.Infrastructure.Helpers.Cqrs.Commands;
using Inventory.Infrastructure.Helpers.Cqrs.Queries;
using Inventory.Infrastructure.Helpers.Events;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Queries;
using Inventory.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddSingleton<IEventBus, EventBus>();

services.AddSingleton<InventoryInMemoryContext>();
services.AddTransient<IItemRepository, ItemInMemoryRepository>();

services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
services.AddSingleton<ICommandDispatcher, CommandDispatcher>();

services.AddTransient<IQueryHandler<GetAllItemsQuery, IEnumerable<Item>>, GetAllItemsQueryHandler>();
services.AddTransient<ICommandHandler<AddItemCommand>, AddItemCommandHandler>();
services.AddTransient<ICommandHandler<RemoveItemByNameCommand>, RemoveItemByNameCommandHandler>();

services.AddScoped<IItemMapper, ItemMapper>();
services.AddScoped<IItemReadService, ItemReadService>();
services.AddScoped<IItemWriteService, ItemWriteService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Subscribe to events
using (var scope = app.Services.CreateScope())
{
    var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
    eventBus.Subscribe<ItemRemovedEvent>(payload =>
    {
        var itemRemoved = payload.Event.Name;
        Console.WriteLine("Item with name {0} has been removed at {1}", itemRemoved, payload.Timestamp);
    });

}

app.MapGet("/items", async () =>
    {
        app.Logger.LogInformation("GET: /items");
        using var scope = app.Services.CreateScope();
        var itemReadService = scope.ServiceProvider.GetRequiredService<IItemReadService>();
        return await itemReadService.GetAllItems();
    })
    .WithName("GetItems");

app.MapPost("/items", async ([FromBody] ItemDto item) =>
    {
        app.Logger.LogInformation("POST: /items");
        using var scope = app.Services.CreateScope();
        var itemWriteService = scope.ServiceProvider.GetRequiredService<IItemWriteService>();
        await itemWriteService.AddItem(item);
        return Results.StatusCode((int)HttpStatusCode.Created);
    })
    .WithName("PostItem");

app.MapDelete("/items/{name}", async (string name) =>
    {
        app.Logger.LogInformation("DELETE: /items/{Name}", name);
        using var scope = app.Services.CreateScope();
        var itemWriteService = scope.ServiceProvider.GetRequiredService<IItemWriteService>();
        try
        {
            await itemWriteService.RemoveItemByName(name);
            return Results.Ok();
        }
        catch (ItemNotFoundException e)
        {
            app.Logger.LogError(e, "Item not found");
            return Results.NotFound();
        }
    })
    .WithName("DeleteItem");

app.Run();