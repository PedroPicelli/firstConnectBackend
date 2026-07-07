
using Models.Dtos.Response;

namespace Models.Dtos.Response;

public class UserResponseDto {
    

    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Bio { get; set; } = string.Empty;

    public ICollection<PostResponseDto> Posts { get; set; } = new List<PostResponseDto>();

    public int LikesCount { get; set; }

}

