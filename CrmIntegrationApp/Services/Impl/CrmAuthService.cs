using CrmIntegrationApp.Configurations;
using CrmIntegrationApp.Helpers;
using CrmIntegrationApp.Models;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace CrmIntegrationApp.Services.Impl
{
    public class CrmAuthService : ICrmAuthService
    {
        private readonly ILogger<CrmAuthService> _logger;

        private readonly HttpClient _httpClient;
        private readonly CrmApiSettings _crmApiSettings;

        private string _accessToken;

        public CrmAuthService(ILogger<CrmAuthService> logger, HttpClient httpClient, IOptions<CrmApiSettings> crmApiOptions)
        {
            _logger = logger;
            _httpClient = httpClient;
            _crmApiSettings = crmApiOptions.Value;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(_accessToken))
            {
                return _accessToken;
            }

            _accessToken = FakeJwtGenerator.GenerateToken("crm_Auth_Server", "integration_app_audience", "super_secret_key_for_webhook_validation");
            return _accessToken;

            //This logic should be executed instead of FakeJwtGenerator but I dont have auth server
            try
            {
                var req = new CrmAuthRequest
                {
                    ClientId = _crmApiSettings.ClientId,
                    ClientSecret = _crmApiSettings.ClientSecret
                };

                var json = JsonSerializer.Serialize(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var requestUrl = $"{_crmApiSettings.BaseUrl}{_crmApiSettings.AuthEndpoint}";
                var response = await _httpClient.PostAsync(requestUrl, content);
                var authResponse = JsonSerializer.Deserialize<CrmAuthResponse>(await response.Content.ReadAsStringAsync());
                _accessToken = authResponse.AccessToken;

                _logger.LogInformation("Successfully received access token.");

                return _accessToken;
            }
            catch (HttpRequestException ex) 
            {
                _logger.LogError(ex, "HTTP request error during Crm token acquisition. Status Code: {StatusCode}. Error: {ErrorMessage}",
                    (int?)ex.StatusCode, ex.Message);
                
                return null;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Failed to get AccessToken.Error: {ErrorMessage}", ex.Message);
                
                return null;
            }
        }
    }
}