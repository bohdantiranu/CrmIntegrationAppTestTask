using System.Net;

namespace CrmIntegrationApp.Exceptions
{
    public class ExternalApiException : CrmIntegrationException
    {
        public HttpStatusCode? StatusCode { get; }
        public string? ResponseBody { get; }

        public ExternalApiException(string message) : base(message) 
        {
        }
        public ExternalApiException(string message, Exception innerException) : base(message, innerException) 
        {
        }
        public ExternalApiException(string message, HttpStatusCode? statusCode, Exception innerException = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
        public ExternalApiException(string message,
            HttpStatusCode? statusCode,
            string? responseBody = null,
            Exception innerException = null) : base(message, innerException)
        {
            StatusCode = statusCode;
            ResponseBody = responseBody;
        }
    }
}
