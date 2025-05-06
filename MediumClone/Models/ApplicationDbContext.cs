using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MediumClone.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Interest> Interests { get; set; }
    }

    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ViewCount { get; set; }
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
        public string? Category { get; set; }
        public string? Username { get; set; }
    }

    public class Interest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<IdentityUser>? Users { get; set; }
    }
}