using CrmIntegrationApp.Exceptions;
using CrmIntegrationApp.Models;

namespace CrmIntegrationApp.Services.Impl
{
    public class WebhookProcessorService  : IWebhookProcessorService
    {
        private readonly ITicketProcessorService _cxoneTicketProcessor;
        private readonly ILogger<WebhookProcessorService> _logger;

        public WebhookProcessorService(ITicketProcessorService cxoneTicketProcessor, ILogger<WebhookProcessorService> logger)
        {
            _cxoneTicketProcessor = cxoneTicketProcessor;
            _logger = logger;
        }

        public async Task ProcessWebhookTicketAsync(CrmTicket crmTicket)
        {
            _logger.LogInformation("Attempting to process Crm ticket {TicketId} from webhook.", crmTicket.Id);
            try
            {
                var isSuccess = await _cxoneTicketProcessor.ProcessAsync(crmTicket);

                if (isSuccess)
                    _logger.LogInformation("Successfully processed ticket {TicketId} from webhook.", crmTicket.Id);

                else
                    throw new TicketProcessingFailedException($"Failed to process ticket {crmTicket.Id} from webhook");
            }
            catch (TicketProcessingFailedException ex)
            {
                _logger.LogError("Failed to process ticket {TicketId} from webhook.", crmTicket.Id);

                throw ;
            }
        }
    }
}
