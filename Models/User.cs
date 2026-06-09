using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagementApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Age is required.")]
        [Range(18, 120, ErrorMessage = "Age must be between 18 and 120.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression("^(Admin|User|Manager)$", ErrorMessage = "Role must be Admin, User, or Manager.")]
        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
