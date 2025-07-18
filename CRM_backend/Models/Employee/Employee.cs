using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CRM_backend.Models.Employee
{
    public class Employee

    {
       [Key]
        public int Id { get; set; }
        // Basic Info
        [Required]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
    ErrorMessage = "Invalid email address format.")]
        public string EmailId { get; set; }


        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime JoiningDate { get; set; } = DateTime.Now;

        public string Type { get; set; }

        public string ProbationPeriod { get; set; }

        public string WorkLocation { get; set; }

        public string Experience { get; set; }

        public string Language { get; set; }

        public string Skills { get; set; }

        [EmailAddress(ErrorMessage = "Invalid personal email address.")]
        public string PersonalMail { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNo { get; set; }

        // Bank Details
        public string BankName { get; set; }

        [RegularExpression("^\\d{9,18}$", ErrorMessage = "Invalid account number.")]
        public string AccountNo { get; set; }

        [RegularExpression("^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Invalid IFSC code format.")]
        public string IFSCCode { get; set; }

        public string Branch { get; set; }

        // Education
        public string Specialization { get; set; }

        public string Qualification { get; set; }

        [RegularExpression("^\\d{4}$", ErrorMessage = "Invalid completion year.")]
        public string CompletionYear { get; set; }

        public string Institute { get; set; }

        // Health and Personal Info
        public string Allergies { get; set; }

        public string BloodGroup { get; set; }

        public string Nationality { get; set; }

        public string DisabilityStatus { get; set; }

        [RegularExpression("^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid PAN format.")]
        public string PANNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        // Address
        public string Street { get; set; }

        public string MaritalStatus { get; set; }

        public string City { get; set; }

        [RegularExpression("^\\d{12}$", ErrorMessage = "Aadhar number must be 12 digits.")]
        public string AadharNumber { get; set; }

        [RegularExpression("^\\d{6}$", ErrorMessage = "ZipCode must be 6 digits.")]
        public string ZipCode { get; set; }

        // Emergency Contact
        public string EmergencyContactName { get; set; }

        public string EmergencyContactRelationship { get; set; }

        [Phone(ErrorMessage = "Invalid emergency contact phone number.")]
        public string EmergencyContactPhoneNo { get; set; }

        [JsonIgnore]
        public ICollection<UserTechnologies> UserTechnologies { get; set; } = new List<UserTechnologies>();
    }
}
