﻿namespace CrmIntegrationApp.Configurations
{
    public class WebhookJwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
    }
}