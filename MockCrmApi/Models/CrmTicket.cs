namespace MockCrmApi.Models
{
    public class CrmTicket
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Tags { get; set; }
        public int Priority { get; set; }
    }
}
