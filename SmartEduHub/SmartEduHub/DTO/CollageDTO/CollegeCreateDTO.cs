using System.ComponentModel.DataAnnotations;

namespace SmartEduHub.DTO.CollageDTO
{
    public class CollegeCreateDTO
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Code { get; set; } = null!;

        public string? Address { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? LogoUrl { get; set; }

        public string? CreatedBy { get; set; }
    }
}
