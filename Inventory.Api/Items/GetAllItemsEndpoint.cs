using System;
using FastEndpoints;
using Inventory.Application.Contracts;
using Inventory.Application.Dto;

namespace Inventory.Api.Items;

public class GetAllItemsEndpoint(ILogger<GetAllItemsEndpoint> logger, IItemReadService itemReadService) 
: Endpoint<GetAllItemsResponse, GetAllItemsResponse>
{
    public override async Task<GetAllItemsResponse> HandleAsync(
        GetAllItemsResponse request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GET: /items");
        var items = await itemReadService.GetAllItems();
        return new GetAllItemsResponse(items);
    }
}

    public record GetAllItemsRequest;

    public record GetAllItemsResponse(IEnumerable<ItemDto> Items);
