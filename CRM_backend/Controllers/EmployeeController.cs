using CRM_backend.DTO;
using CRM_backend.Models;
using CRM_backend.Repositories;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CRM_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepo _employeeRepo;

        public EmployeeController(IEmployeeRepo employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        /// <summary>
        /// Adds a new employee to the system.
        /// </summary>
        /// <param name="employeeDto">Employee DTO to be added.</param>
        /// <returns>Created employee data or error.</returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(Employee), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var employee = employeeDto.Adapt<Employee>();
                await _employeeRepo.AddAsync(employee);
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets an employee by ID.
        /// </summary>
        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(Employee), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEmployee([FromRoute][Range(1, int.MaxValue)] int id)
        {
            try
            {
                var employee = await _employeeRepo.GetByIdAsync(e => e.Id == id);
                if (employee == null)
                    return NotFound($"Employee with ID {id} not found.");

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets all employees.
        /// </summary>
        [HttpGet("getall")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeRepo.GetAllAsync();
                if (employees == null || !employees.Any())
                    return NotFound("No employees found.");

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        [HttpPut("update/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existing = await _employeeRepo.GetByIdAsync(e => e.Id == id);
                if (existing == null)
                    return NotFound($"Employee with ID {id} not found.");

                Employee employee = employeeDto.Adapt<Employee>();

                await _employeeRepo.UpdateAsync(employee);
                return Ok("Employee updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes an employee by ID.
        /// </summary>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteEmployee([FromRoute][Range(1, int.MaxValue)] int id)
        {
            
            try
            {
                var existing = await _employeeRepo.GetByIdAsync(e => e.Id == id);
                if (existing == null)
                    return NotFound($"Employee with ID {id} not found.");

                await _employeeRepo.DeleteAsync(id);
                return Ok("Employee deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
