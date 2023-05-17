using Journeys.ApplicationServices.Journeys;
using Journeys.Core.Journeys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Journeys.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JourneysController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IJourneysAppService _journeysAppService;
        public JourneysController (IJourneysAppService journeysAppService, ILogger<JourneysController> logger)
        {
            _journeysAppService = journeysAppService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        // GET: api/<JourneysController>
        [HttpGet]
        public async Task<List<Journey>> GetAll()
        {
            try
            {
                List<Journey> journeys = await _journeysAppService.GetJourneysAsync();

                _logger.LogInformation("Total journeys: {Count}", journeys?.Count ?? 0);

                return journeys ?? new List<Journey>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving journeys.");
                throw; // Reenvía la excepción para que pueda ser manejada en un nivel superior si es necesario.
            }
        }


        // GET api/<JourneysController>/5
        [HttpGet("{id}")]
        public async Task<Journey> Get(int id)
        {
            try
            {
                Journey journey = await _journeysAppService.GetJourneyAsync(id);

                _logger.LogInformation("Get Journey with ID: {Id}", id);

                return journey;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving journey with ID {Id}", id);
                throw;
            }
        }


        // POST api/<JourneysController>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Journey value)
        {
            try
            {
                await _journeysAppService.AddJourneyAsync(value);
                _logger.LogInformation("New journey has been added");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new journey.");
                return StatusCode(500, "An error occurred while adding a new journey. Please try again later.");
            }
        }


        // PUT api/<JourneysController>/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Journey value)
        {
            try
            {
                value.Id = id;
                await _journeysAppService.EditJourneyAsync(value);
                _logger.LogInformation("Edit Journey with ID: {Id}", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while editing journey with ID {Id}", id);
                return StatusCode(500, "An error occurred while editing the journey. Please try again later.");
            }
        }


        // DELETE api/<JourneysController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _journeysAppService.DeleteJourneyAsync(id);
                _logger.LogInformation("Delete Journey with ID: {Id}", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting journey with ID {Id}", id);
                return StatusCode(500, "An error occurred while deleting the journey. Please try again later.");
            }
        }

    }
}
