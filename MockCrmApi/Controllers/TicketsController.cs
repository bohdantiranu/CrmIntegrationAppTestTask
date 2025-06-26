using Microsoft.AspNetCore.Mvc;
using MockCrmApi.Models;

namespace MockCrmApi.Controllers
{
    [ApiController]
    [Route("api/crm/tickets")]
    public class TicketsController : ControllerBase
    {
        private readonly List<CrmTicket> _tickets = new List<CrmTicket>();

        public TicketsController()
        {
            var ticket1 = new CrmTicket
            {
                Id = "Crm_1",
                Title = "title1",
                Body = "body1",
                CreatedAt = DateTime.Now,
                Priority = 1
            };

            var ticket2 = new CrmTicket
            {
                Id = "Crm_2",
                Title = "title2",
                Body = "body2",
                CreatedAt = DateTime.Now.AddMinutes(5),
                Priority = 2
            };
            _tickets.Add(ticket1);
            _tickets.Add(ticket2);
        }

        [HttpGet]
        public IActionResult GetTickets()
        {
            return Ok(_tickets);
        }
    }
}