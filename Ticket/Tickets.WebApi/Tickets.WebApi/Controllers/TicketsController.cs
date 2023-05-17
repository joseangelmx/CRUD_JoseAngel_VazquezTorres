using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;
        public TicketsController(ITicketsAppService ticketsAppService,
            ILogger<TicketsController> logger)
        {
            _ticketsAppService = ticketsAppService;
            _logger = logger;
        }
        // GET: api/<TicketsController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Ticket> tickets = await _ticketsAppService.GetTicketsAsync();

                if (tickets == null || tickets.Count == 0)
                {
                    return NoContent();
                }

                _logger.LogInformation($"Total tickets: {tickets.Count}");
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving all tickets: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve tickets");
            }
        }

        // GET api/<TicketsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                Ticket ticket = await _ticketsAppService.GetTicketAsync(id);

                if (ticket == null)
                {
                    return NotFound();
                }

                _logger.LogInformation($"Get Ticket {id}");
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving the ticket with id {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve ticket");
            }
        }

        // POST api/<TicketsController>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Ticket value)
        {
            if (value == null)
            {
                return BadRequest("Invalid ticket data");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _ticketsAppService.AddTicketAsync(value);
                _logger.LogInformation($"New Ticket {value.Id} has been added");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while adding a new ticket: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add ticket");
            }
        }

        [Authorize]
        // PUT api/<TicketsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Ticket value)
        {
            if (value == null)
            {
                return BadRequest("Invalid ticket data");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                value.Id = id;
                await _ticketsAppService.EditTicketAsync(value);
                _logger.LogInformation($"Ticket with id {value.Id} has been edited");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while editing the ticket with id {value.Id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to edit ticket");
            }
        }

        // DELETE api/<TicketsController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _ticketsAppService.DeleteTicketAsync(id);
                _logger.LogInformation($"Ticket with id {id} has been deleted");
                return Ok(new { status = "success", message = $"Ticket with id {id} has been deleted" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting the ticket with id {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "error", message = $"Failed to delete ticket with id {id}" });
            }
        }
    }
}
