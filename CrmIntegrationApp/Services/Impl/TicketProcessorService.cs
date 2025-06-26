using AutoMapper;
using CrmIntegrationApp.Exceptions;
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

            try
            {
                var cxoneTicket = _mapper.Map<CxoneTicket>(crmTicket);
                var isSuccess = await _cxoneApiClient.SendTicketAsync(cxoneTicket);

                if (!isSuccess)
                {
                    _logger.LogError("Failed to process ticket {TicketId}.", crmTicket.Id);

                    throw new TicketProcessingFailedException($"Failed to process ticket ticket {crmTicket.Id}.");
                }

                _logger.LogInformation("Successfully processed ticket {TicketId}.", crmTicket.Id);

                return isSuccess;
            }
            catch (AuthenticationFailedException ex)
            {
                _logger.LogError("Failed to get CXone access token. Cant send ticket.");

                throw new TicketProcessingFailedException("Failed to get CXone access token. Cant send ticket.", ex);
            }
            catch (ExternalApiException ex)
            {
                _logger.LogError(ex, "CXone API call failed during processing ticket {TicketId}. Status: {StatusCode}", crmTicket.Id, ex.StatusCode);

                throw new TicketProcessingFailedException($"CXone API call failed during processing ticket {crmTicket.Id}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process ticket {TicketId}.", crmTicket.Id);
                
                throw new TicketProcessingFailedException($"Failed to process ticket {crmTicket.Id}.", ex);
            }
        }
    }
}
