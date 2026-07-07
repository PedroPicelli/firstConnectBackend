using System.Reflection.Metadata.Ecma335;
using Data;
using Microsoft.EntityFrameworkCore;
using Models.Database;
using Models.Dtos.Create;
using Models.Dtos.Response;

namespace Services;


public class PostService {
    
    private readonly AppDbContext _db;

    public PostService(AppDbContext db) {
        _db = db;
    }



    async public Task<Post> CreatePostAsync(CreatePostDto createDto, int userId) {
        
        var post = new Post {
            UserId = userId,
            Content = createDto.Content
        };

        await _db.Posts.AddAsync(post);
        await _db.SaveChangesAsync();

        return post;

    }


    async public Task<List<PostResponseDto>> GetPostsAsync(int userId) {
        
        var posts = await _db.Posts
        .AsNoTracking()
        .OrderByDescending(p => p.CreatedAt)
        .Select(p => new PostResponseDto {
            
            Id = p.Id,
            Content = p.Content,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            UserId = p.UserId,
            Username = p.User.Username,
            DisplayName = p.User.DisplayName,
            LikesCount = p.Likes.Count,
            IsLikedByMe = p.Likes.Any(pl => pl.UserId == userId),
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
                .ToList()

        })
        .ToListAsync();


        return posts;

    }


    async public Task<PostResponseDto?> GetPostByIdAsync(int id, int userId) {
        
        var post = await _db.Posts
        .AsNoTracking()
        .Where(p => p.Id == id)
        .Select(p => new PostResponseDto {
            
            Id = p.Id,
            Content = p.Content,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            UserId = p.UserId,
            Username = p.User.Username,
            DisplayName = p.User.DisplayName,
            LikesCount = p.Likes.Count,
            IsLikedByMe = p.Likes.Any(pl => pl.UserId == userId),

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
                .ToList()

        })
        .FirstOrDefaultAsync();


        return post;

    }




    public async Task<(bool isLiked, int totalLikes)> ToggleLikeAsync(int postId, int userId) {
        
        var postExists = await _db.Posts.AnyAsync(p => p.Id == postId);
        if(!postExists) {
            throw new KeyNotFoundException("Post Not Found");
        }

        var existingLike = await _db.PostLikes
            .FirstOrDefaultAsync(pl => pl.PostId == postId && pl.UserId == userId);

        
        bool isLiked;

        if(existingLike != null) {
            _db.PostLikes.Remove(existingLike);
            isLiked = false;

        } else {
            var newLike = new PostLike {
                PostId = postId,
                UserId = userId
            };
            await _db.PostLikes.AddAsync(newLike);
            isLiked = true;
        }

        await _db.SaveChangesAsync();

        int totalLikes = await _db.PostLikes.CountAsync(pl => pl.PostId == postId);


        return (isLiked, totalLikes);

    }


    public async Task<Comment> CreateCommentAsync(CreateCommentDto createDto, int postId, int userId) {
        
        var postExists = await _db.Posts.AnyAsync(p => p.Id == postId);
        if(!postExists) {
            throw new KeyNotFoundException("Post Not Found");
        }


        var newComment = new Comment {
            
            UserId = userId,
            PostId = postId,
            Content = createDto.Content

        };

        await _db.Comments.AddAsync(newComment);
        await _db.SaveChangesAsync();

        return newComment;

    }


    public async Task<List<CommentResponseDto>> GetCommentsAsync(int postId) {
        
        var comments = await _db.Comments
            .AsNoTracking()
            .Where(c => c.PostId == postId)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new CommentResponseDto {
                
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                PostId = c.PostId,
                UserId = c.UserId,
                Username = c.User.Username,
                DisplayName = c.User.DisplayName,

            })
            .ToListAsync();


        return comments;

    }

}

