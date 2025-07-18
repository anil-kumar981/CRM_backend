using CRM_backend.Models.Employee;
using CRM_backend.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnologiesController : ControllerBase
    {
        private readonly ITechnologyRepo _technologyRepo;
        public TechnologiesController(ITechnologyRepo technologyRepo)
        {
            _technologyRepo = technologyRepo;
        }
        /// <summary>
        /// Gets all technologies.
        /// </summary>
        [HttpGet("getall")]
        [ProducesResponseType(typeof(IEnumerable<Technologies>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllTechnologies()
        {
            try
            {
                var technologies = await _technologyRepo.GetAllAsync();
                return Ok(technologies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        /// <summary>
        /// Gets a technology by ID.
        /// </summary>
        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(Technologies), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTechnology([FromRoute] int id)
        {
            try
            {
                var technology = await _technologyRepo.GetByIdAsync(t => t.Id == id);
                if (technology == null)
                {
                    return NotFound($"Technology with ID {id} not found.");
                }
                return Ok(technology);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        /// <summary>
        /// Creates a new technology.
        /// </summary>
        [HttpPost("Create")]
        public async Task<IActionResult> CreateTechnology([FromBody] Technologies technology)
        {

            if (technology == null || string.IsNullOrEmpty(technology.Name))
            {
                return BadRequest("Invalid technology data.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _technologyRepo.AddAsync(technology);
                return CreatedAtAction(nameof(GetTechnology), new { id = technology.Id }, technology);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        /// <summary>
        /// Deletes a technology by ID.
        /// </summary>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTechnology([FromRoute] int id)
        {
            await _technologyRepo.DeleteAsync(id);
            return Ok($"Technology with ID {id} deleted successfully.");
        }
    }
}
