using BlogAPI.Storage.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BlogAPI.Storage.InMemory;

public class InMemoryDbContext : DbContext
{
    public DbSet<Blog>? Blogs { get; set; }
    public DbSet<Comment>? Comments { get; set; }
    public DbSet<Post>? Posts { get; set; }

    public InMemoryDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.ToTable("Blogs");
            entity.HasKey("ID");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Posts");
            entity.HasKey("ID");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comments");
            entity.HasKey("ID");
        });

        base.OnModelCreating(modelBuilder);
    }
}


