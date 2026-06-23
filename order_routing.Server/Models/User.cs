using System.ComponentModel.DataAnnotations;

namespace order_routing.Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public Store? Store { get; set; }
        public int? StoreId { get; set; }
    }
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MinLength(4, ErrorMessage = "Ο κωδικός πρέπει να είναι τουλάχιστον 4 χαρακτήρες.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "StoreUser";

        public int? StoreId { get; set; }
    }

    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
    public class UserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int? StoreId { get; set; }
    }
}
