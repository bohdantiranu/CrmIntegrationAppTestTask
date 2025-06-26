using CrmIntegrationApp.Configurations;
using CrmIntegrationApp.Models;
using CrmIntegrationApp.Services;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CrmIntegrationApp.Infrastructure.ApiClients.Impl
{
    public class CrmApiClient : ICrmApiClient
    {
        private readonly ICrmAuthService _crmAuthService;
        private readonly ILogger<CrmApiClient> _logger;

        private readonly HttpClient _httpClient;
        private readonly CrmApiSettings _crmApiSettings;

        public CrmApiClient(ICrmAuthService crmAuthService, ILogger<CrmApiClient> logger, HttpClient httpClient, IOptions<CrmApiSettings> crmApiOptions)
        {
            _crmAuthService = crmAuthService;
            _logger = logger;
            _httpClient = httpClient;
            _crmApiSettings = crmApiOptions.Value;
        }

        public async Task<IEnumerable<CrmTicket>> GetTicketsAsync()
        {
            if (string.IsNullOrEmpty(_crmApiSettings.BaseUrl))
            {
                _logger.LogError("Failed to fetch tickets. Crm API base url is not set.");

                return new List<CrmTicket>();
            }

            string requestUrl = $"{_crmApiSettings.BaseUrl}/tickets";

            _logger.LogInformation("Attempting to fetch tickets from Crm. Request URL: {RequestUrl}", requestUrl);

            try
            {
                var token = await _crmAuthService.GetAccessTokenAsync();
                
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogError("Failed to get Crm access token. Cant fetch tickets.");
                    
                    return new List<CrmTicket>();
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                var response = await _httpClient.GetAsync(requestUrl);
                var responseData = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Received response from Crm. Status Code: {StatusCode}.",
                    (int)response.StatusCode);

                response.EnsureSuccessStatusCode();

                var crmTickets = JsonSerializer.Deserialize<List<CrmTicket>>(responseData,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                _logger.LogInformation("Successfully fetched {TicketCount} tickets from Crm.", crmTickets?.Count ?? 0);

                return crmTickets ?? new List<CrmTicket>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed. Error: {ErrorMessage}", ex.Message);

                return new List<CrmTicket>();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize response data. Error: {ErrorMessage}", ex.Message);

                return new List<CrmTicket>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error. Error: {ErrorMessage}", ex.Message);

                return new List<CrmTicket>();
            }
        }
    }
}