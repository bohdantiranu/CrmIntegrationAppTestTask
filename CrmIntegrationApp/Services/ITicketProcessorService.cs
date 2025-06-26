using CrmIntegrationApp.Models;

namespace CrmIntegrationApp.Services
{
    public interface ITicketProcessorService
    {
        public Task<bool> ProcessAsync(CrmTicket crmTicket);
    }
}
