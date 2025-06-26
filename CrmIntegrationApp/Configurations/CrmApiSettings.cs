namespace CrmIntegrationApp.Configurations
{
    public class CrmApiSettings
    {
        public string BaseUrl { get; set; }
        public string AuthEndpoint { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public int PollingIntervalInHours {  get; set; }
    }
}