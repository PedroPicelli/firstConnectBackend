using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Models.Database;
using Models.Dtos.Create;
using Models.Dtos.Response;
using Services;

namespace Controllers;


[ApiController]
[Route("posts")]
public class PostController : ControllerBase {
    
    private readonly PostService _postService;

    public PostController(PostService postService) {
        
        _postService = postService;

    }


    [Authorize]
    [HttpPost()]
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
            LikesCount = 0,
            IsLikedByMe = false,
            Username = username!,
            DisplayName = displayName!
        };


        return Created("", postResponse);
    }


    [Authorize]
    [HttpGet]
    async public Task<IActionResult> GetPosts() {
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(userId == null) {
            return Unauthorized();
        }


        var posts = await _postService.GetPostsAsync(int.Parse(userId));


        return Ok(posts);

    }


    [Authorize]
    [HttpGet("{id:int}")]
    async public Task<IActionResult> GetPostById(int id) {
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(userId == null) {
            return Unauthorized();
        }


        var post = await _postService.GetPostByIdAsync(id, int.Parse(userId));

        if(post == null) {
            return NotFound();
        }


        return Ok(post);

    }


    [Authorize]
    [HttpPost("{postId}/like")]
    public async Task<IActionResult> ToggleLike(int postId) {
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(userId == null) {
            return Unauthorized();
        }

        try {
            
            var (isLiked, totalLikes) = await _postService.ToggleLikeAsync(postId, int.Parse(userId));

            return Ok(new { isLiked, totalLikes });

        } catch(KeyNotFoundException) {
            
            return NotFound();

        }

    }


    [Authorize]
    [HttpPost("{postId}/comments")]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto createDto, int postId) {
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(userId == null) {
            return Unauthorized();
        }

        try {
            var comment = await _postService.CreateCommentAsync(createDto, postId, int.Parse(userId));
            

            var username = User.FindFirst("username")?.Value;
            var displayName = User.FindFirst("displayName")?.Value;
            var commentResponse = new CommentResponseDto {

                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
                PostId = comment.PostId,
                UserId = comment.UserId,
                Username = username!,
                DisplayName = displayName!,

            };

            return Created("", commentResponse);

        } catch(KeyNotFoundException) {
            return NotFound();
        }

    }


    [Authorize]
    [HttpGet("{postId}/comments")]
    public async Task<IActionResult> GetComments(int postId) {
        
        var comments = await _postService.GetCommentsAsync(postId);

        return Ok(comments);

    }

}
