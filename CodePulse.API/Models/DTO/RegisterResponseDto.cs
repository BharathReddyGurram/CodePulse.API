namespace CodePulse.API.Models.DTO
{
    public class RegisterResponseDto
    {

        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? UserId { get; set; }
    }
}
