using System.ComponentModel.DataAnnotations;

namespace SmartEduHub.DTO
{
    public class RegisterDTO
    {
        public string? Username { get; set; }

        [StringLength(13, MinimumLength = 10, ErrorMessage = "Phone number must be 10 digit.")]
        public string? PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string? Role { get; set; }
        public string? Email { get; set; }
        public int? CollegeId { get; set; }
    }
}
