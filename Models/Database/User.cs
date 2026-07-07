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
    [MaxLength(150)]
    public string Bio { get; set; } = string.Empty;


    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public ICollection<Post> Posts { get; set; } = new List<Post>();

    public ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();


    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;


    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;


    public DateTimeOffset UpdatedAt { get; set; }
}


