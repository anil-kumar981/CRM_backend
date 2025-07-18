using CRM_backend.Models.Employee;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CRM_backend.Models.Project
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        

        [DataType(DataType.Date)]
        public DateTime? ExpectedDeliveryDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Budget must be a positive value")]
        public decimal? Budget { get; set; }

        [MaxFileSize(20 * 1024 * 1024)] // 20 MB limit
        public byte[] Document { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL")]
        public string AnyLinks { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public ProjectStatus Status { get; set; }
        [JsonIgnore]
        public ICollection<ProjectTechnology> ProjectTechnologies { get; set; } = new List<ProjectTechnology>();

    }
    public enum ProjectStatus
    {
        NotStarted=0,
        InProgress=1,
        Completed=2,
        OnHold=3,
        Cancelled=4
    }
}