
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


    async public Task<User> GetUserAsync(int userId) {
        
        var user = await _db.Users.FirstOrDefaultAsync(user => user.Id == userId);

        if(user == null) {
            
            throw new KeyNotFoundException("User not found");

        }

        return user;
    }

    async public Task<User> EditProfileAsync(EditProfileDto editDto, int userId) {
        
        var user = await GetUserAsync(userId);

        user.DisplayName = editDto.DisplayName;
        user.Bio = editDto.Bio;


        return user;

    }


}

