using Microsoft.EntityFrameworkCore;
using Models.Database;

namespace Data;


public class AppDbContext : DbContext {
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostLike> PostLikes { get; set; }
    public DbSet<Comment> Comments { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(user => user.Username)
            .IsUnique();



        modelBuilder.Entity<PostLike>()
            .HasKey(pl => new { pl.UserId, pl.PostId });

        modelBuilder.Entity<PostLike>()
            .HasOne(pl => pl.Post)
            .WithMany(p => p.Likes)
            .HasForeignKey(pl => pl.PostId);
    }
}

