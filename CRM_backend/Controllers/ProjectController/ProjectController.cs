using CRM_backend.DTO.ProjectDtos;
using CRM_backend.Models.Project;
using CRM_backend.Repositories;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CRM_backend.Controllers.ProjectController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepo _projectRepo;
        private readonly IWebHostEnvironment _env;

        public ProjectController(IProjectRepo projectRepo, IWebHostEnvironment env)
        {
            _projectRepo = projectRepo;
            _env = env;
        }

        /// <summary>
        /// Get all projects.
        /// </summary>
        /// <response code="200">Returns list of all projects.</response>
        /// <response code="404">If no projects are found.</response>
        [HttpGet("all-projects")]
        [ProducesResponseType(typeof(IEnumerable<Project>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectRepo.GetAllAsync();
            return !projects.Any() ? NotFound("No projects found.") : Ok(projects);
        }

        /// <summary>
        /// Get project by ID.
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <response code="200">Returns the project with given ID.</response>
        /// <response code="404">If project is not found.</response>
        [HttpGet("get-project-by-id/{id}")]
        [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _projectRepo.GetByIdAsync(p => p.Id == id);
            return project == null ? NotFound("Project not found.") : Ok(project);
        }

        /// <summary>
        /// Create a new project.
        /// </summary>
        /// <param name="dto">Project data with document and technology IDs</param>
        /// <response code="201">Returns the newly created project.</response>
        /// <response code="400">If model state is invalid.</response>
        [HttpPost("create-project")]
        [ProducesResponseType(typeof(Project), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProject([FromForm] ProjectDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var project = dto.Adapt<Project>();

            if (dto.Document != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "documents");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(dto.Document.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                await dto.Document.CopyToAsync(fileStream);

                project.Document = $"/documents/{uniqueFileName}";
            }

            await _projectRepo.AddWithTechnologiesAsync(project, dto.TechnologyIds);

            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
        }

        /// <summary>
        /// Update an existing project.
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <param name="dto">Updated project data</param>
        /// <response code="204">Project updated successfully.</response>
        /// <response code="404">If project is not found.</response>
        [HttpPut("update-project/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProject(int id, [FromForm] ProjectDto dto)
        {
            var existing = await _projectRepo.GetByIdAsync(p => p.Id == id);
            if (existing == null)
                return NotFound("Project not found.");

            dto.Adapt(existing);

            if (dto.Document != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "documents");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(dto.Document.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                await dto.Document.CopyToAsync(fileStream);

                existing.Document = $"/documents/{uniqueFileName}";
            }

            await _projectRepo.UpdateWithTechnologiesAsync(id, existing, dto.TechnologyIds);

            return NoContent();
        }

        /// <summary>
        /// Delete a project.
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <response code="204">Project deleted successfully.</response>
        [HttpDelete("delete-project/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProject(int id)
        {
            await _projectRepo.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Download the project document.
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <response code="200">Returns the project document file.</response>
        /// <response code="404">If project or file is not found.</response>
        [HttpGet("download/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadDocument(int id)
        {
            var project = await _projectRepo.GetByIdAsync(p => p.Id == id);

            if (project == null || string.IsNullOrEmpty(project.Document))
                return NotFound("Project or document not found.");

            var filePath = Path.Combine(_env.WebRootPath, project.Document.TrimStart('/'));

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found on disk.");

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);//read data into byte form
            var provider = new FileExtensionContentTypeProvider();//built in class used for define the type of file
            provider.TryGetContentType(filePath, out string contentType);//used here 
            contentType ??= "application/octet-stream";//contentType defines the MIME type. application/octet-stream is a generic binary file type.

            var fileName = Path.GetFileName(filePath);
            return File(fileBytes, contentType, fileName);
        }
    }
}
