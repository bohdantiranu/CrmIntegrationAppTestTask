
using CrmIntegrationApp.Configurations;
using CrmIntegrationApp.Helpers;
using Microsoft.Extensions.Options;

namespace CrmIntegrationApp.Services.Impl
{
    public class CxoneAuthService : ICxoneAuthService
    {
        private readonly ILogger<CxoneAuthService> _logger;

        private readonly HttpClient _httpClient;
        private readonly CxoneApiSettings _cxoneApiSettings;

        private string _accessToken;

        public CxoneAuthService(ILogger<CxoneAuthService> logger, HttpClient httpClient, IOptions<CxoneApiSettings> cxoneApiOptions)
        {
            _logger = logger;
            _httpClient = httpClient;
            _cxoneApiSettings = cxoneApiOptions.Value;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            //similar logic to CrmAuthService but also no Auth Server so Im using FakeJwtGenerator
            _accessToken = FakeJwtGenerator.GenerateToken("cxone_Auth_Server", "integration_app_audience", "super_secret_key_for_cxone_validation");
            
            return _accessToken;
        }
    }
}
