using StoryDragon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace StoryDragon.Data
{
    // Open-Closed Principle (OCP): The context is open for extension by inheriting from IdentityDbContext<User>
    public class StoryDragonContext : IdentityDbContext<User>
    {
        // Single Responsibility Principle (SRP): The context has a single responsibility of defining the database sets
        public DbSet<Story> Stories { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Post> Posts { get; set; }

        // Constructor injection (Dependency Inversion Principle)
        public StoryDragonContext(DbContextOptions<StoryDragonContext> options) : base(options)
        {
        }

        // Single Responsibility Principle (SRP): The OnModelCreating method has a single responsibility of configuring the database model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the many-to-many relationship between Story and Character
            modelBuilder.Entity<Story>()
                .HasMany(s => s.Characters)
                .WithMany(c => c.Stories)
                .UsingEntity(j => j.ToTable("StoryCharacters"));

            // Configuring the one-to-many relationship between Post and Story
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Story)
                .WithMany(s => s.Posts)
                .HasForeignKey(p => p.StoryId);

            // Configuring the one-to-many relationship between Post and Character
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Character)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CharacterId);

            // Configuring the one-to-many relationship between User and Post with cascading delete
            modelBuilder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuring the one-to-many relationship between User and Character with cascading delete
            modelBuilder.Entity<User>()
                .HasMany(u => u.Characters)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuring the one-to-many relationship between User and Story with cascading delete
            modelBuilder.Entity<User>()
                .HasMany(u => u.Stories)
                .WithOne()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuring the composite key for IdentityUserRole
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(ur => new { ur.UserId, ur.RoleId });

            // Calling the base OnModelCreating method
            base.OnModelCreating(modelBuilder);
        }
    }
}