using System.ComponentModel.DataAnnotations;

namespace TayanaYacht.Models
{
    public class AdminUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(200)]
        public string PasswordHash { get; set; }
    }
}