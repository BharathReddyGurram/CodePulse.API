namespace CodePulse.API.Models.Domain
{
    public enum UserRole
    {
        SuperAdmin,
        Manager,
        User
    }

    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User; // default
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
