using CRM_backend.Models.Project;
using System.ComponentModel.DataAnnotations;

namespace CRM_backend.DTO.ProjectDtos
{
    public class ProjectDto
    {

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }



        [DataType(DataType.Date)]
        public DateTime? ExpectedDeliveryDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Budget must be a positive value")]
        public decimal? Budget { get; set; }

        [MaxFileSize(20 * 1024 * 1024)] // 20 MB limit
        public IFormFile? Document { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL")]
        public string AnyLinks { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public ProjectStatus Status { get; set; }
        [Required(ErrorMessage = "At least one technology must be selected.")]
        public List<int> TechnologyIds { get; set; } = new();
    }

}