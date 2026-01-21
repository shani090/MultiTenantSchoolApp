namespace SmartEduHub.DTO.RegisterResponseDTO
{
    public class SecureEndpointResponseDto
    {
        public string Message { get; set; } = "";
        public string Username { get; set; } = "";
        public string Role { get; set; } = "";
        public string UserId { get; set; } = "";
        public string CollegeId { get; set; } = "";
        public string CollegeName { get; set; } = "";
        public object? CollegeInfo { get; set; }
        public object? UserInfo { get; set; }
    }
}
