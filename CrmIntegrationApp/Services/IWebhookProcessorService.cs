using CrmIntegrationApp.Models;

namespace CrmIntegrationApp.Services
{
    public interface IWebhookProcessorService
    {
        public Task ProcessWebhookTicketAsync(CrmTicket crmTicket);
    }
}
