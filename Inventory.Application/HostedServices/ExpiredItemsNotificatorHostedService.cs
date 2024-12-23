using Inventory.Application.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.HostedServices;

public sealed class ExpiredItemsNotificatorHostedService : IHostedService, IDisposable
{
    private readonly IItemReadService _itemReadService;
    private readonly ILogger<ExpiredItemsNotificatorHostedService> _logger;
    private Timer? _timer;

    public ExpiredItemsNotificatorHostedService(IItemReadService itemReadService,
        ILogger<ExpiredItemsNotificatorHostedService> logger)
    {
        _itemReadService = itemReadService;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Initializing hosted service");
        _timer = new Timer(Notify, null, TimeSpan.Zero, TimeSpan.FromHours(24));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    void IDisposable.Dispose()
    {
        GC.SuppressFinalize(this);
        _timer?.Dispose();
    }

    private async void Notify(object? state)
    {
        var itemsNotified = await _itemReadService.NotifyExpiredItems();
        _logger.LogInformation("{ItemsNotified} items have been notified", itemsNotified);
    }
}