using Karshare.API.Validators;
using System.ComponentModel.DataAnnotations;

namespace Karshare.API.DTOs
{
    public class UserInfoDTO
    {

        [Required]
        [RegularExpression(@"^[A-Z].*", ErrorMessage = "FirstName must start with an Uppercase Letter !")]
        public string? FirstName { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z\-]*", ErrorMessage = "LastName must be only Uppercase !")]
        public string? LastName { get; set; }
        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? City { get; set; }
        [Required]
        public string? Country { get; set; }
        public int Age { get; set; }
        public bool HasLicense { get; set; }
        public int YearsOfLicense { get; set; }

        public string? Address {  get; set; }
    }
}
