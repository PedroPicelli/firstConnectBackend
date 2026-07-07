using System.ComponentModel.DataAnnotations;


namespace Models.Dtos.Response;


public class CommentResponseDto {
    public int Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public int UserId { get; set; }
    public int PostId { get; set; }

    public string Username { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;
    
}


