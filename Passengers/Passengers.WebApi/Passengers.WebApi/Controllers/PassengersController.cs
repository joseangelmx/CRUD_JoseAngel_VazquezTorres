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
            List<Passenger> passengers = await _passengersAppService.GetPassengersAsync();
            _logger.LogInformation("Total passengers: " + passengers?.Count);
            return (passengers);
        }

        // GET api/<PassengersController>/5
        [HttpGet("{id}")]
        public async Task<Passenger> Get(int id)
        {
            Passenger passenger = await _passengersAppService.GetPassengerAsync(id);
            _logger.LogInformation("Get Passenger " + id);
            return passenger;
        }

        // POST api/<PassengersController>
        [HttpPost]
        public async Task Insert([FromBody] Passenger value)
        {

            await _passengersAppService.AddPassengerAsync(value);
            _logger.LogInformation("Get Passenger " + value.FirstName+" "+value.LastName);
        }

        // PUT api/<PassengersController>/5
        [HttpPut("{id}")]
        public async Task Edit(int id, [FromBody] Passenger value)
        {
            value.Id = id;
            await _passengersAppService.EditPassengerAsync(value);
            _logger.LogInformation("Edit Passenger " + value.Id);
        }

        // DELETE api/<PassengersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _passengersAppService.DeletePassengerAsync(id);
            _logger.LogInformation("Delete Passenger " + id);
            return Ok();
        }
    }
}
