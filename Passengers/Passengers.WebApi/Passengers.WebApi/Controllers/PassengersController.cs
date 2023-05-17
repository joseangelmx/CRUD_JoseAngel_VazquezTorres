using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Passengers.ApplicationServices.Passengers;
using Passengers.Core.Passengers;
using Passengers.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Passengers.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengersController : ControllerBase
    {

        private readonly ILogger _logger;
        private readonly IPassengersAppService _passengersAppService;
        public PassengersController(IPassengersAppService passengerAppService, ILogger<PassengersController> logger)
        {
            _passengersAppService = passengerAppService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
        }
        // GET: api/<PassengersController>
        [HttpGet]
        public async Task<List<Passenger>> GetAll()
        {
            try
            {
                List<Passenger> passengers = await _passengersAppService.GetPassengersAsync();
                _logger.LogInformation("Total passengers: {Count}", passengers?.Count);
                return passengers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving passengers.");
                throw; 
            }
        }


        // GET api/<PassengersController>/5
        [HttpGet("{id}")]
        public async Task<Passenger> Get(int id)
        {
            Passenger passenger = await _passengersAppService.GetPassengerAsync(id);

            if (passenger != null)
            {
                _logger.LogInformation("Get Passenger {Id}", id);
                return passenger;
            }
            else
            {
                _logger.LogWarning("Passenger with ID {Id} not found", id);
                return null;
            }
        }


        // POST api/<PassengersController>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Insert([FromBody] Passenger value)
        {
            try
            {
                await _passengersAppService.AddPassengerAsync(value);
                _logger.LogInformation("Inserted Passenger: {FullName}", $"{value.FirstName} {value.LastName}");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while inserting passenger.");
                return StatusCode(500, "An error occurred while inserting the passenger. Please try again later.");
            }
        }


        // PUT api/<PassengersController>/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Passenger value)
        {
            try
            {
                value.Id = id;
                await _passengersAppService.EditPassengerAsync(value);
                _logger.LogInformation("Edited Passenger: {Id}", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while editing passenger.");
                return StatusCode(500, "An error occurred while editing the passenger. Please try again later.");
            }
        }


        // DELETE api/<PassengersController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _passengersAppService.DeletePassengerAsync(id);
                _logger.LogInformation("Deleted Passenger: {Id}", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting passenger.");
                return StatusCode(500, "An error occurred while deleting the passenger. Please try again later.");
            }
        }

    }
}
