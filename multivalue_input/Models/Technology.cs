using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace multivalue_input.Models
{
    public class Technology
    {
        [Key]
    
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<UserTechnology> UserTechnologies { get; set; }= new List<UserTechnology>();

    }
}
