
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using multivalue_input.db;
using multivalue_input.Models;

namespace multivalue_input.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnologiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TechnologiesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Name))
            {
                return BadRequest("Invalid technology data.");
            }
            await _context.Users.AddRangeAsync(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
        // GET: api/users/{id}
        [HttpGet("user/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPost("CreatedTechnology")]
        public async Task<IActionResult> CreateTechnology([FromBody] Technology technology)
        {
            if (technology == null || string.IsNullOrEmpty(technology.Name))
            {
                return BadRequest("Invalid technology data.");
            }
            await _context.Technologies.AddAsync(technology);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTechnologies), new { id = technology.Id }, technology);
        }
        // GET: api/technologies
        [HttpGet("technologies")]
        public async Task<ActionResult<Technology>> GetTechnologies()
        {
            var val = await _context.Technologies.ToListAsync();
            return Ok(val);
        }
        // GET: api/technologies/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Technology>> GetTechnology(int id)
        {
            var technology = await _context.Technologies.FindAsync(id);
            if (technology == null)
            {
                return NotFound();
            }
            return Ok(technology);
        }
        [HttpGet("user-technologies")]
        public async Task<ActionResult<IEnumerable<UserTechnology>>> GetUserTechnologies()
        {
            var userTechnologies = await _context.UserTechnologies
                .Include(ut => ut.User)
                .Include(ut => ut.Technology)
                .ToListAsync();
            return Ok(userTechnologies);
        }
        [HttpPost("CreateUserTechnology" +
            "")]
        public async Task<IActionResult> CreateUserTechnology([FromBody] UserTechonolgyDTO userTechnology)
        {
            if ((userTechnology==null))
            {
                return BadRequest("not found");
            }
            var existing = await _context.UserTechnologies
                               .Where(u => u.UserId == userTechnology.UserId)
                               .ToListAsync();
            _context.UserTechnologies.RemoveRange(existing);

            foreach (var techId in userTechnology.TechnologyIds)
            {
                var newRecord = new UserTechnology
                {
                    UserId = userTechnology.UserId,
                    TechnologyId = techId
                };
                await _context.UserTechnologies.AddAsync(newRecord);
            }
            await _context.SaveChangesAsync();
            return Ok("User technologies updated successfully.");
        }
    }
}