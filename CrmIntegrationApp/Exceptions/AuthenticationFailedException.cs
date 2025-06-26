namespace CrmIntegrationApp.Exceptions
{
    public class AuthenticationFailedException : CrmIntegrationException
    {
        public AuthenticationFailedException() 
        {
        }
        public AuthenticationFailedException(string message) : base(message) 
        {
        }
        public AuthenticationFailedException(string message, Exception innerException) : base(message, innerException) 
        {
        }
    }

}
