using System.ComponentModel.DataAnnotations;


namespace Models.Dtos.Create;


public class CreateAccountDto {
    
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;


    [Required]
    [MinLength(2)]
    [MaxLength(32)]
    public string DisplayName { get; set; } = string.Empty;


    [Required]
    [MinLength(3)]
    [MaxLength(32)]
    [RegularExpression(
        @"^[a-z0-9._]+$",
        ErrorMessage = "Username contains invalid characters."
    )]
    public string Username { get; set; } = string.Empty;


    [Required]
    [MinLength(8)]
    [MaxLength(128)]
    public string Password { get; set; } = string.Empty;

}


