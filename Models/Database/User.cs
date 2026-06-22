using System.ComponentModel.DataAnnotations;

namespace Models.Database;


public class User {
    
    [Key]
    public int Id { get; set; }


    [Required]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;


    [Required]
    [MaxLength(32)]
    public string DisplayName { get; set; } = string.Empty;


    [Required]
    [MaxLength(32)]
    public string Username { get; set; } = string.Empty;


    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public DateTime UpdatedAt { get; set; }
}


