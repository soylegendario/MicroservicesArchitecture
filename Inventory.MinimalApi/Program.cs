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
using Inventory.MinimalApi.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[] { }
        }
    });
});

services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
        ("BasicAuthentication", null);
services.AddAuthorization();

services.AddSingleton<IEventBus, EventBus>();

services.AddSingleton<InventoryInMemoryContext>();
services.AddTransient<IItemRepository, ItemInMemoryRepository>();

services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
services.AddSingleton<ICommandDispatcher, CommandDispatcher>();

services.AddTransient<IQueryHandler<GetAllItemsQuery, IEnumerable<Item>>, GetAllItemsQueryHandler>();
services.AddTransient<IQueryHandler<GetItemsByExpirationDateQuery, IEnumerable<Item>>, GetItemsByExpirationDateQueryHandler>();
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
app.UseAuthentication();
app.UseAuthorization();

// Subscribe to events
using (var scope = app.Services.CreateScope())
{
    var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
    eventBus.Subscribe<ItemRemovedEvent>(payload =>
    {
        Console.WriteLine("Item with name {0} has been removed at {1}", payload.Event.Name, payload.Timestamp);
    });
    eventBus.Subscribe<ItemExpiredEvent>(payload =>
    {
        Console.WriteLine("Item with name {0} has expired at {1}", payload.Event.Name, payload.Event.ExpirationDate);
    });
}

// Schedule a job to notify expired items once a day
var timer = new System.Timers.Timer(24 * 60 * 60 * 1000);
timer.Elapsed += async (_, _) =>
{
    try
    {
        app.Logger.LogInformation("Executing scheduled job to notify expired items");
        using var scope = app.Services.CreateScope();
        var itemReadService = scope.ServiceProvider.GetRequiredService<IItemReadService>();
        var itemsNotified = await itemReadService.NotifyExpiredItems();
        app.Logger.LogInformation("{ItemsNotified} items have been notified", itemsNotified);
    }
    catch (Exception e)
    {
        app.Logger.LogError(e, "Error while executing scheduled job to notify expired items");
    }
};
timer.Enabled = true;
timer.Start();

app.MapGet("/items", 
        [Authorize] 
        async ([FromServices] IItemReadService itemReadService) =>
    {
        app.Logger.LogInformation("GET: /items");
        return await itemReadService.GetAllItems();
    })
    .WithName("GetItems");

app.MapPost("/items", 
        [Authorize] 
        async ([FromBody] ItemDto item, [FromServices] IItemWriteService itemWriteService) =>
    {
        app.Logger.LogInformation("POST: /items");
        await itemWriteService.AddItem(item);
        return Results.StatusCode((int)HttpStatusCode.Created);
    })
    .WithName("PostItem");

app.MapDelete("/items/{name}", 
        [Authorize] 
        async (string name, [FromServices] IItemWriteService itemWriteService) =>
    {
        try
        {
            app.Logger.LogInformation("DELETE: /items/{Name}", name);
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