using System.ComponentModel.DataAnnotations;

namespace CRM_backend.DTO
{
    public class UserTechnologiesDto
    {
        [Required(ErrorMessage = "UserId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "At least one technology must be selected.")]
        [MinLength(1, ErrorMessage = "At least one technology must be selected.")]
        public List<int> TechnologyIds { get; set; }
    }
}
