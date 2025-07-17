using System.ComponentModel.DataAnnotations;

namespace multivalue_input.Models
{
    public class UserTechnology
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int TechnologyId { get; set; }

        public User User { get; set; }
        public Technology Technology { get; set; }
    }
}
