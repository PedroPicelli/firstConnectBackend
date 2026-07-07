using System.ComponentModel.DataAnnotations;


namespace Models.Dtos.Create;


public class CreateCommentDto {

    [Required]
    [MaxLength(500)]
    public string Content { get; set; } = string.Empty;

}


