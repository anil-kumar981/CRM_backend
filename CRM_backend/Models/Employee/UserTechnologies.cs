using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CRM_backend.Models.Employee
{
    public class UserTechnologies
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "TechnologyId is required.")]
        public int TechnologyId { get; set; }

        public Employee Employee { get; set; }
        public Technologies Technologies { get; set; }
    }
}
