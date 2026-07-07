using System.ComponentModel.DataAnnotations;

namespace Models.Database;



public class Comment {
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int PostId { get; set; }
    public Post Post { get; set; } = null!;

    [Required]
    [MaxLength(500)]
    public string Content { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTime.UtcNow;
}