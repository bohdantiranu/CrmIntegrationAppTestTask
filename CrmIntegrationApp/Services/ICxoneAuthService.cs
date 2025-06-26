namespace CrmIntegrationApp.Services
{
    public interface ICxoneAuthService
    {
        public Task<string> GetAccessTokenAsync();
    }
}
