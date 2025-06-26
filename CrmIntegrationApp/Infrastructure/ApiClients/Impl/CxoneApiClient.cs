using CrmIntegrationApp.Configurations;
using CrmIntegrationApp.Exceptions;
using CrmIntegrationApp.Models;
using CrmIntegrationApp.Services;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CrmIntegrationApp.Infrastructure.ApiClients.Impl
{
    public class CxoneApiClient : ICxoneApiClient
    {
        private readonly ICxoneAuthService _authService;
        private readonly ILogger<CxoneApiClient> _logger;

        private readonly HttpClient _httpClient;
        private readonly CxoneApiSettings _cxoneApiSettings;

        public CxoneApiClient(ICxoneAuthService cxoneAuthService, 
            ILogger<CxoneApiClient> logger, 
            HttpClient httpClient, 
            IOptions<CxoneApiSettings> cxoneApiOptions)
        {
            _authService = cxoneAuthService;
            _logger = logger;
            _httpClient = httpClient;
            _cxoneApiSettings = cxoneApiOptions.Value;
        }

        public async Task<bool> SendTicketAsync(CxoneTicket ticket)
        {
            try
            {
                var token = await _authService.GetAccessTokenAsync();
                
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogError("Failed to get CXone access token. Cant send ticket.");

                    throw new AuthenticationFailedException("Failed to get CXone access token. Cant send ticket.");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var json = JsonSerializer.Serialize(ticket);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogInformation("Attempting to send ticket {TicketId} to CXone. Request URL: {RequestUrl}", 
                    ticket.TicketId, _cxoneApiSettings.BaseUrl);
                _logger.LogDebug("Ticket {TicketId} payload: {Payload}", ticket.TicketId, json);

                var response = await _httpClient.PostAsync(_cxoneApiSettings.BaseUrl, content);

                _logger.LogInformation("Received response from CXone for ticket {TicketId}. Status Code: {StatusCode}.",
                    ticket.TicketId, (int)response.StatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();

                    _logger.LogError("Failed to send ticket {TicketId} to CXone . Status Code: {StatusCode}. Error: {ErrorDescription}",
                        ticket.TicketId, (int)response.StatusCode, error);

                    throw new ExternalApiException($"CXone API call failed while sending ticket.", response.StatusCode, error);
                }

                _logger.LogInformation("Ticket {TicketId} successfully sent to CXone.", ticket.TicketId);

                return true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed for ticket {TicketId}. Error: {ErrorMessage}.", 
                    ticket.TicketId, ex.Message);

                throw new ExternalApiException($"HTTP request failed.", ex.StatusCode, ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to serialization ticket {TicketId}. Error: {ErrorMessage}", ticket.TicketId, ex.Message);

                throw new TicketProcessingFailedException("Failed to serialization ticket {TicketId}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred. Ticket {TicketId}. Error: {ErrorMessage}", ticket.TicketId, ex.Message);

                throw new CrmIntegrationException($"An unexpected error.", ex);
            }
        }
    }
}