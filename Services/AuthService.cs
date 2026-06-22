
using System.Text.RegularExpressions;
using Data;
using Microsoft.EntityFrameworkCore;
using Models.Database;
using Models.Dtos.Create;

namespace Services;

public class AuthService {
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db) {
        _db = db;
    }

    
    async public Task<User?> CreateUserAsync(CreateAccountDto createDto) {
        
        var normalizedEmail = createDto.Email.Trim().ToLower();

        var normalizedUsername = createDto.Username.Trim().ToLower();

        var displayName = createDto.DisplayName.Trim();

        if(string.IsNullOrWhiteSpace(displayName))
            return null;



        var isInfoUsed = await _db.Users.AnyAsync(user => user.Email == normalizedEmail || user.Username == normalizedUsername);

        if(isInfoUsed)
            return null;


        var user = new User {

            Email = normalizedEmail,
            DisplayName = displayName,
            Username = normalizedUsername,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(createDto.Password),

            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };


        _db.Users.Add(user);
        try {
            await _db.SaveChangesAsync();
        }
        catch(DbUpdateException) {
            return null;
        }

        return user;

    }


    async public Task<bool> IsUsernameAvailable(string username) {

        var usernameNormalized = username.Trim().ToLower();
        
        var isUsed = await _db.Users.AnyAsync(user => user.Username == usernameNormalized);


        return !isUsed;

    }

    async public Task<bool> isEmailAvailable(string email) {

        var emailNormalized = email.Trim().ToLower();

        if(string.IsNullOrWhiteSpace(emailNormalized))
            return false;


        string pattern = @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$";

        if(!Regex.IsMatch(emailNormalized, pattern, RegexOptions.IgnoreCase))
            return false;


        
        var isUsed = await _db.Users.AnyAsync(user => user.Email == emailNormalized);


        return !isUsed;

    }

}

