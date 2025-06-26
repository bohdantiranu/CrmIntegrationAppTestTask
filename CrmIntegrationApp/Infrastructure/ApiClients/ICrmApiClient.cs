using CrmIntegrationApp.Models;

namespace CrmIntegrationApp.Infrastructure.ApiClients
{
    public interface ICrmApiClient
    {
        public Task<IEnumerable<CrmTicket>> GetTicketsAsync();
    }
}
