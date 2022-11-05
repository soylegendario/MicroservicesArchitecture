using System.Net;
using Inventory.Application;
using Inventory.Application.Contracts;
using Inventory.Application.Dto;
using Inventory.CrossCutting.Events;
using Inventory.CrossCutting.Exceptions;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Events;
using Inventory.MinimalApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services
    .ConfigureApiServices()
    .ConfigureInfrastructureServices()
    .ConfigureApplicationServices();

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
app.UseMiddleware<GlobalExceptionHandler>();

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
        app.Logger.LogInformation("POST: /items");
        await itemWriteService.AddItem(item);
        return Results.StatusCode((int)HttpStatusCode.Created);
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
        app.Logger.LogInformation("DELETE: /items/{Name}", name);
        await itemWriteService.RemoveItemByName(name);
        return Results.Ok();
    })
    .WithName("DeleteItem");

app.Run();