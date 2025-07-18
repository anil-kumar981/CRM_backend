using CRM_backend.Models.Employee;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CRM_backend.Models.Project
{
    public class ProjectTechnology
    {
      
        [JsonIgnore]
        public int Id { get; set; }
        [Required(ErrorMessage = "ProjectId is required.")]
        public int ProjectId { get; set; }
        [Required(ErrorMessage = "TechnologyId is required.")]
        public int TechnologyId { get; set; }
        public  Project Project { get; set; }
        public  Technologies Technology { get; set; }

      
    }
}
