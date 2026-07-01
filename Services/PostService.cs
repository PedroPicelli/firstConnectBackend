using Data;
using Models.Database;
using Models.Dtos.Create;

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


}

