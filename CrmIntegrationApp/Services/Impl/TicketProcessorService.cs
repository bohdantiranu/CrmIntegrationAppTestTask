using AutoMapper;
using CrmIntegrationApp.Infrastructure.ApiClients;
using CrmIntegrationApp.Models;

namespace CrmIntegrationApp.Services.Impl
{
    public class TicketProcessorService : ITicketProcessorService
    {
        private readonly ICxoneApiClient _cxoneApiClient;
        private readonly IMapper _mapper;
        private readonly ILogger<TicketProcessorService> _logger;

        public TicketProcessorService(ICxoneApiClient cxoneApiClient, IMapper mapper, ILogger<TicketProcessorService> logger)
        {
            _cxoneApiClient = cxoneApiClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> ProcessAsync(CrmTicket crmTicket)
        {
            _logger.LogInformation("Attempting to process Crm ticket {TicketId}.", crmTicket.Id);

            var cxoneTicket = _mapper.Map<CxoneTicket>(crmTicket);
            var isSuccess = await _cxoneApiClient.SendTicketAsync(cxoneTicket);

            if (isSuccess)
                _logger.LogInformation("Successfully processed ticket {TicketId}.", crmTicket.Id);

            else
                _logger.LogError("Failed to process ticket {TicketId}.", crmTicket.Id);

            return isSuccess;
        }
    }
}
