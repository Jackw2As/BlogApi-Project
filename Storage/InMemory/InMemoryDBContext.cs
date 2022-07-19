using BlogAPI.Storage.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Storage.InMemory
{
    public class InMemoryDBContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }

        public InMemoryDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Blog>(entity => entity.ToTable("Blogs"));
            modelBuilder.Entity<Post>(entity => entity.ToTable("Posts"));
            modelBuilder.Entity<Comment>(entity => entity.ToTable("Comments"));
        }
    }
}
