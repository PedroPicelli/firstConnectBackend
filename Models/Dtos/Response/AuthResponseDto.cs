
namespace Models.Dtos.Response;

public class AuthResponseDto {
    

    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

}
