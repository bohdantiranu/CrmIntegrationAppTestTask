using CrmIntegrationApp.Configurations;
using CrmIntegrationApp.Exceptions;
using Microsoft.Extensions.Options;

namespace CrmIntegrationApp.Services.Impl
{
    public class TicketBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TicketBackgroundService> _logger;
        private readonly TimeSpan _pollingIntervalInHours;

        public TicketBackgroundService(IServiceScopeFactory scopeFactory, 
            ILogger<TicketBackgroundService> logger,
            IOptions<CrmApiSettings> settings)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _pollingIntervalInHours = TimeSpan.FromHours(settings.Value.PollingIntervalInHours);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Ticket background service started.");
            
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var ticketService = scope.ServiceProvider.GetRequiredService<ITicketService>();
                        await ticketService.ProcessTicketsAsync();
                    }

                    _logger.LogInformation("Poll complited. Next run in {Interval} hours.", _pollingIntervalInHours.TotalHours);

                }
                catch (ExternalApiException ex)
                {
                    _logger.LogError(ex, "Periodic polling failed during crm API call.Next run in {Interval} hours.", _pollingIntervalInHours.TotalHours);
                }
                catch (TicketProcessingFailedException ex)
                {
                    _logger.LogError(ex, "Periodic polling failed during processing requested tickets. Next run in {Interval} hours.", _pollingIntervalInHours.TotalHours);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Periodic polling failed. Next run in {Interval} hours.", _pollingIntervalInHours.TotalHours);
                }

                await Task.Delay(_pollingIntervalInHours, stoppingToken);
            }

            _logger.LogInformation("Ticket background service stopped.");
        }
    }
}