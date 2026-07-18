
using System.Text.RegularExpressions;
using Data;
using Microsoft.EntityFrameworkCore;
using Models.Database;
using Models.Dtos.Create;
using Models.Dtos.Edit;
using Models.Dtos.Login;
using Models.Dtos.Response;

namespace Services;

public class UserService {
    private readonly AppDbContext _db;

    public UserService(AppDbContext db) {
        _db = db;
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

    async public Task<User> GetUserAsync(int userId) {
        
        var user = await _db.Users.FirstOrDefaultAsync(user => user.Id == userId);

        if(user == null) {
            
            throw new KeyNotFoundException("User not found");

        }

        return user;
    }


    async public Task<User> GetFullUserAsync(int userId) {
        
        var user = await _db.Users
            .Include(u => u.Posts)
                .ThenInclude(p => p.Likes)
            .Include(u => u.Posts)
                .ThenInclude(p => p.Comments)
            .FirstOrDefaultAsync(user => user.Id == userId);

        if(user == null) {
            
            throw new KeyNotFoundException("User not found");

        }

        return user;
    }

    async public Task<User> EditProfileAsync(EditProfileDto editDto, int userId) {
        
        var user = await GetUserAsync(userId);

        user.DisplayName = editDto.DisplayName;
        user.Bio = editDto.Bio;

        await _db.SaveChangesAsync();

        return user;

    }


}

