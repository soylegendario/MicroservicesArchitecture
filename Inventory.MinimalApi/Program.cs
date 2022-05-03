using System.Net;
using FluentValidation;
using Inventory.Api.Authentication;
using Inventory.Application.Contracts;
using Inventory.Application.Dto;
using Inventory.Application.Mappers.Items;
using Inventory.Application.Services;
using Inventory.Application.Validators;
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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;

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
            Array.Empty<string>()
        }
    });
});

services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
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

services.AddTransient<AbstractValidator<ItemDto>, ItemValidator>();

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
        [SwaggerOperation("Get all items")]
        [SwaggerResponse(200, Description = "Operation success", Type = typeof(IEnumerable<ItemDto>))]
        [SwaggerResponse(401, Description = "Unauthorized")]
        [SwaggerResponse(500, Description = "Unexpected error")]
        async ([FromServices] IItemReadService itemReadService) =>
    {
        app.Logger.LogInformation("GET: /items");
        return await itemReadService.GetAllItems();
    })
    .WithName("GetItems");

app.MapPost("/items", 
        [Authorize] 
        [SwaggerOperation("Create a new item")]
        [SwaggerResponse(201, Description = "Created")]
        [SwaggerResponse(400, Description = "Bad request")]
        [SwaggerResponse(401, Description = "Unauthorized")]
        [SwaggerResponse(500, Description = "Unexpected error")]
        async ([FromBody] ItemDto item, [FromServices] IItemWriteService itemWriteService) =>
    {
        try
        {
            app.Logger.LogInformation("POST: /items");
            await itemWriteService.AddItem(item);
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        catch (ValidationException e)
        {
            app.Logger.LogError(e, "POST: /items, validation error");
            return Results.BadRequest(e.Message);
        }
    })
    .WithName("PostItem");

app.MapDelete("/items/{name}", 
        [Authorize] 
        [SwaggerOperation("Delete a item by name")]
        [SwaggerResponse(200, Description = "Operation success")]
        [SwaggerResponse(400, Description = "Bad request")]
        [SwaggerResponse(401, Description = "Unauthorized")]
        [SwaggerResponse(404, Description = "Not found")]
        [SwaggerResponse(500, Description = "Unexpected error")]
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