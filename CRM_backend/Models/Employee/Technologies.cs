using CRM_backend.Models.Project;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CRM_backend.Models.Employee
{
    public class Technologies
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Technology name is required.")]
        [StringLength(50, ErrorMessage = "Name length can't exceed 50 characters.")]
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<UserTechnologies> UserTechnologies { get; set; }
        [JsonIgnore]
        public ICollection<ProjectTechnology> ProjectTechnologies { get; set; }
    }
}
