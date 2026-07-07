using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Create;
using Models.Dtos.Response;
using Models.Dtos.Login;
using Services;
using Models.Dtos.Edit;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Models.Database;

namespace Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase {
    private readonly UserService _userService;
    private readonly TokenService _tokenService;


    public UserController(UserService userService, TokenService tokenService) {
        
        _userService = userService;
        _tokenService = tokenService;

    }


    [Authorize]
    [HttpGet("me")]
    async public Task<IActionResult> GetUser() {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(userId == null) {
            return Unauthorized();
        }
        
        try {
            var user = await _userService.GetUserAsync(int.Parse(userId));

            return Ok(getUserResponseDto(user));

        } catch(KeyNotFoundException) {
            return NotFound();
        }
    }



    [Authorize]
    [HttpPut("me")]
    async public Task<IActionResult> EditProfile([FromBody] EditProfileDto editDto) {
    
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(userId == null) {
            return Unauthorized();
        }

        try {
            var user = await _userService.EditProfileAsync(editDto, int.Parse(userId));


            var token = _tokenService.GenerateToken(user);

            return Ok(new {
                Token = token,
                User = getUserResponseDto(user)
            });

        } catch(KeyNotFoundException) {
            
            return NotFound();

        }

    }



    public UserResponseDto getUserResponseDto(User user) {
        var userResponse = new UserResponseDto {
            Id = user.Id,
            Email = user.Email,
            DisplayName = user.DisplayName,
            Username = user.Username,
            Bio = user.Bio,
            Posts = user.Posts
                .OrderByDescending(c => c.CreatedAt) 
                .Select(p => new PostResponseDto {
                    Id = p.Id,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    UserId = p.UserId,
                    IsLikedByMe = p.Likes.Any(pl => p.UserId == pl.UserId),
                    LikesCount = p.Likes.Count(),
                    
                    Comments = p.Comments
                        .OrderByDescending(c => c.CreatedAt) 
                        .Select(c => new CommentResponseDto {
                            Id = c.Id,
                            Content = c.Content,
                            CreatedAt = c.CreatedAt,
                            UpdatedAt = c.UpdatedAt,
                            PostId = c.PostId,
                            UserId = c.UserId,
                            Username = c.User.Username,
                            DisplayName = c.User.DisplayName
                        })
                        .ToList(),
                    Username = p.User.Username,
                    DisplayName = p.User.DisplayName
                })
                .ToList(),

            LikesCount = user.PostLikes.Count

        };

        return userResponse;
    }


}

