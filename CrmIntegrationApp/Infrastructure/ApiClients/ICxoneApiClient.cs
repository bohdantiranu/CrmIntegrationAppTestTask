using CrmIntegrationApp.Models;

namespace CrmIntegrationApp.Infrastructure.ApiClients
{
    public interface ICxoneApiClient
    {
        public Task<bool> SendTicketAsync(CxoneTicket ticket);
    }
}
