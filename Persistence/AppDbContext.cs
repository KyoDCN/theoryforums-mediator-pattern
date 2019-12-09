using Domain.Identity;
using Domain.Forum;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class AppDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ForumCategory> ForumCategories { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ForumCategory>().HasKey(x => x.Id);
            builder.Entity<Forum>().HasKey(x => x.Id);
            builder.Entity<Thread>().HasKey(x => x.Id);
            builder.Entity<Post>().HasKey(x => x.Id);

            builder.Entity<ForumCategory>()
                .HasMany(e => e.Forums)
                .WithOne(e => e.ForumCategory)
                .HasForeignKey(x => x.ForumCategoryFK)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Forum>()
                .HasMany(e => e.Threads)
                .WithOne(e => e.Forum)
                .HasForeignKey(x => x.ForumFK)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Thread>()
                .HasMany(e => e.Posts)
                .WithOne(e => e.Thread)
                .HasForeignKey(x => x.ThreadFK)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasMany(e => e.Posts)
                .WithOne(e => e.Author)
                .HasForeignKey(x => x.AuthorFK)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<User>()
                .HasMany(e => e.Threads)
                .WithOne(e => e.Author)
                .HasForeignKey(x => x.AuthorFK)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
