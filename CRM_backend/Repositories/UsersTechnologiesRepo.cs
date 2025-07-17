using CRM_backend.DB;
using CRM_backend.DTO;
using CRM_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM_backend.Repositories
{
    /// <summary>
    /// Repository for managing user-technologies assignments.
    /// </summary>
    public class UsersTechnologiesRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsersTechnologiesRepo> _logger;

        public UsersTechnologiesRepo(ApplicationDbContext context, ILogger<UsersTechnologiesRepo> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all user-technology assignments with navigation properties.
        /// </summary>
        public async Task<ActionResult<IEnumerable<UserTechnologies>>> GetUserTechnologies()
        {
            try
            {
                return await _context.UserTechnologies
                    .Include(ut => ut.Employee)
                    .Include(ut => ut.Technologies)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all user-technologies.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieves a specific user-technology assignment by ID.
        /// </summary>
        public async Task<ActionResult<UserTechnologies>> GetUserTechnologyById(int id)
        {
            try
            {
                var userTechnology = await _context.UserTechnologies
                    .Include(ut => ut.Employee)
                    .Include(ut => ut.Technologies)
                    .FirstOrDefaultAsync(ut => ut.Id == id);

                if (userTechnology == null)
                {
                    return new NotFoundObjectResult($"User technology with ID {id} not found.");
                }

                return userTechnology;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving user-technology with ID {id}.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Adds new user-technology assignments (avoids duplicates).
        /// </summary>
        public async Task<ActionResult> AddUserTechnology(UserTechnologiesDto userTechnologiesDto)
        {
            if (userTechnologiesDto == null)
                return new BadRequestObjectResult("Request payload cannot be null.");

            try
            {
                foreach (var techId in userTechnologiesDto.TechnologyIds)
                {
                    bool alreadyExists = await _context.UserTechnologies
                        .AnyAsync(ut => ut.UserId == userTechnologiesDto.UserId && ut.TechnologyId == techId);

                    if (!alreadyExists)
                    {
                        var userTechnology = new UserTechnologies
                        {
                            UserId = userTechnologiesDto.UserId,
                            TechnologyId = techId,
                        };
                        await _context.UserTechnologies.AddAsync(userTechnology);
                    }
                }

                await _context.SaveChangesAsync();
                return new OkObjectResult("User technologies added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding user-technologies.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates user-technology assignments by adding new ones. Existing ones are kept.
        /// </summary>
        public async Task<IActionResult> UpdateAsync(UserTechnologiesDto userTechnologiesDto)
        {
            if (userTechnologiesDto == null)
                return new BadRequestObjectResult("Invalid request data.");

            try
            {
                var existingTechs = await _context.UserTechnologies
                    .Where(ut => ut.UserId == userTechnologiesDto.UserId)
                    .ToListAsync();

                var incomingTechIds = userTechnologiesDto.TechnologyIds;

                foreach (var techId in incomingTechIds)
                {
                    bool alreadyExists = existingTechs.Any(et => et.TechnologyId == techId);
                    if (!alreadyExists)
                    {
                        var newEntry = new UserTechnologies
                        {
                            UserId = userTechnologiesDto.UserId,
                            TechnologyId = techId
                        };
                        await _context.UserTechnologies.AddAsync(newEntry);
                    }
                }

                await _context.SaveChangesAsync();
                return new OkObjectResult("User technologies updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user-technologies.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieves technologies not yet assigned to the specified user.
        /// </summary>
        public async Task<ActionResult<IEnumerable<Technologies>>> GetAvailableTechnologiesForUser(int userId)
        {
            try
            {
                var assignedTechIds = await _context.UserTechnologies
                    .Where(ut => ut.UserId == userId)
                    .Select(ut => ut.TechnologyId)
                    .ToListAsync();

                var unassignedTechnologies = await _context.Technologies
                    .Where(t => !assignedTechIds.Contains(t.Id))
                    .ToListAsync();

                return unassignedTechnologies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving unassigned technologies for user ID {userId}.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
