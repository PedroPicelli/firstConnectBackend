using System.ComponentModel.DataAnnotations;


namespace Models.Dtos.Login;

public enum LoginIdentifierType {
    Email,
    Username
}

public class LoginAccountDto {
    [Required]
    public string loginIdentifier { get; set; } = string.Empty;


    [Required]
    public LoginIdentifierType loginIdentifierType { get; set; }


    [Required]
    public string Password { get; set; } = string.Empty;

}


