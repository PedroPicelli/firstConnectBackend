using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Create;
using Services;

namespace Controllers;


[ApiController]
[Route("post")]
public class PostController : ControllerBase {
    
    private readonly PostService _postService;

    public PostController(PostService postService) {
        
        _postService = postService;

    }


    [Authorize]
    [HttpPost("create")]
    async public Task<IActionResult> CreatePost(CreatePostDto createDto) {
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(userId == null) {
            return Unauthorized();
        }

        var post = await _postService.CreatePostAsync(createDto, int.Parse(userId));


        var username = User.FindFirst("username")?.Value;
        var displayName = User.FindFirst("displayName")?.Value;
        var postResponse = new PostResponseDto {
            Id = post.Id,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            UserId = post.UserId,
            Username = username!,
            DisplayName = displayName!
        };


        return Created("", postResponse);
    }


}

