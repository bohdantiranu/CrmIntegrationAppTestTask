namespace CrmIntegrationApp.Services
{
    public interface ICrmAuthService
    {
        public Task<string> GetAccessTokenAsync();
    }
}
