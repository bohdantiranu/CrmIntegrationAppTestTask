namespace CrmIntegrationApp.Exceptions
{
    public class TicketProcessingFailedException : CrmIntegrationException
    {
        public TicketProcessingFailedException() 
        {
        }
        public TicketProcessingFailedException(string message) : base(message) 
        {
        }
        public TicketProcessingFailedException(string message, Exception innerException) : base(message, innerException) 
        { 
        }
    }
}