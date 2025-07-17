using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace multivalue_input.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }  // e.g., Identity UserId
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<UserTechnology> UserTechnologies { get; set; }= new List<UserTechnology>();

    }
}
