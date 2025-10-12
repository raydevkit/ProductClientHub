namespace ProductClientHub.API.Entities
{
    public class User : EntityBase
    {
        public string Email { get; set; } = string.Empty; // unique via DB constraint
        public string Name { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
    }
}