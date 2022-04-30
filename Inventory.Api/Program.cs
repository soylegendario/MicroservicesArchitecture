using Inventory.Domain.Items;
using Inventory.Infrastructure.Commands;
using Inventory.Infrastructure.Helpers.Cqrs.Commands;
using Inventory.Infrastructure.Helpers.Cqrs.Queries;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Queries;
using Inventory.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddSingleton<InventoryInMemoryContext>();
services.AddScoped<IItemRepository, ItemInMemoryRepository>();
services.AddScoped<IQueryHandler<GetAllItemsQuery, IEnumerable<Item>>, GetAllItemsQueryHandler>();
services.AddScoped<ICommandHandler<AddItemCommand>, AddItemCommandHandler>();
services.AddScoped<ICommandHandler<RemoveItemByNameCommand>, RemoveItemByNameCommandHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/items", () =>
    {
        app.Logger.LogInformation("GET: /items");
        return new[] { "item1", "item2" };

    })
.WithName("GetItems");

app.Run();