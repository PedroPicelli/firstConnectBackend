using System.ComponentModel.DataAnnotations;


namespace Models.Dtos.Create;


public class CreatePostDto {
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;
    
}


