using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tickets.ApplicationServices.Tickets;
using Tickets.Core.Tickets;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tickets.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketsAppService _ticketsAppService;
        public TicketsController(ITicketsAppService ticketsAppService)
        {
            _ticketsAppService = ticketsAppService;
        }
        // GET: api/<TicketsController>
        [HttpGet]
        public async Task<List<Ticket>> GetAll()
        {
            List<Ticket> tickets = await _ticketsAppService.GetTicketsAsync();
            return (tickets);
        }

        // GET api/<TicketsController>/5
        [HttpGet("{id}")]
        public async Task<Ticket> Get(int id)
        {
            Ticket ticket = await _ticketsAppService.GetTicketAsync(id);

            return ticket;
        }

        // POST api/<TicketsController>
        [HttpPost]
        public async Task Insert([FromBody] Ticket value)
        {

            await _ticketsAppService.AddTicketAsync(value);

        }

        // PUT api/<TicketsController>/5
        [HttpPut("{id}")]
        public async Task Edit(int id, [FromBody] Ticket value)
        {
            value.Id = id;
            await _ticketsAppService.EditTicketAsync(value);
        }

        // DELETE api/<TicketsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ticketsAppService.DeleteTicketAsync(id);
            return Ok();
        }
    }
}
