namespace CrmIntegrationApp.Models
{
    public class CxoneTicket
    {
        public string TicketId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string Priority { get; set; }
        public List<string> Tags { get; set; }
    }
}