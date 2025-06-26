using CrmIntegrationApp.Infrastructure.ApiClients;

namespace CrmIntegrationApp.Services.Impl
{
    public class TicketService : ITicketService
    {
        private readonly ICrmApiClient _crmApiClient;
        private readonly ITicketProcessorService _ticketProcessor;
        private readonly ILogger<TicketService> _logger;

        public TicketService(ICrmApiClient crmApiClient, ITicketProcessorService ticketProcessor, ILogger<TicketService> logger)
        {
            _crmApiClient = crmApiClient;
            _ticketProcessor = ticketProcessor;
            _logger = logger;
        }

        public async Task ProcessTicketsAsync()
        {
            _logger.LogInformation("Starting periodic ticket polling from Crm.");

            var crmTickets = await _crmApiClient.GetTicketsAsync();

            if (crmTickets == null || !crmTickets.Any())
            {
                _logger.LogWarning("No tickets received.");
                
                return;
            }

            _logger.LogInformation("Received {TicketCount} tickets.", crmTickets.Count());

            foreach (var crmTicket in crmTickets)
            {
                _logger.LogInformation("Processing ticket {TicketId}.", crmTicket.Id);
                
                await _ticketProcessor.ProcessAsync(crmTicket);
            }
        }
    }
}