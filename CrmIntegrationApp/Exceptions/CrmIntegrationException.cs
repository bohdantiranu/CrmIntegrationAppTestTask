namespace CrmIntegrationApp.Exceptions
{
    public class CrmIntegrationException : Exception
    {
        public CrmIntegrationException() 
        {
        }
        public CrmIntegrationException(string message) : base(message) 
        {
        }
        public CrmIntegrationException(string message, Exception innerException) : base(message, innerException) 
        {
        }
    }
}
