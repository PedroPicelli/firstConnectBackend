using System.ComponentModel.DataAnnotations;


namespace Models.Dtos.Edit;


public class EditProfileDto {

    [Required]
    [MinLength(3)]
    [MaxLength(32)]
    public string DisplayName { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Bio { get; set; } = string.Empty;

}


