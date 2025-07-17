using CRM_backend.DTO;
using CRM_backend.Models;
using CRM_backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CRM_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersTechnologiesController : ControllerBase
    {
        private readonly UsersTechnologiesRepo _repo;
        private readonly ILogger<UsersTechnologiesController> _logger;

        public UsersTechnologiesController(UsersTechnologiesRepo repo, ILogger<UsersTechnologiesController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        /// <summary>
        /// Get all user technologies with employee and technology details.
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _repo.GetUserTechnologies();
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching user technologies.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Get user technology by its ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _repo.GetUserTechnologyById(id);
                if (result.Result is NotFoundObjectResult)
                    return NotFound(result.Result);
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching technology with ID {id}");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Add user technologies (one-to-many mapping).
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] UserTechnologiesDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _repo.AddUserTechnology(dto);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user technologies.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Update user technologies by adding new ones (does not remove old).
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UserTechnologiesDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _repo.UpdateAsync(dto);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user technologies.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Get technologies not yet assigned to a given user.
        /// </summary>
        [HttpGet("available-technologies/{userId}")]
        public async Task<IActionResult> GetAvailableTechnologies(int userId)
        {
            try
            {
                var result = await _repo.GetAvailableTechnologiesForUser(userId);
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting available technologies for user {userId}");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
