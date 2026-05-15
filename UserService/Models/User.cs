using System;
using System.ComponentModel.DataAnnotations;

namespace UserService.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        // Stored as byte[] by migrations
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        [Required]
        public string Role { get; set; } = "User";
    }
}
