using BlogAPI.Storage.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BlogAPI.Storage.InMemory;

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
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.ToTable("Blogs");

            entity.Property(e => e.PostIds).HasConversion(new IEnumerableGuidToString());
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Posts");

            entity.Property(e => e.CommentIds).HasConversion(new IEnumerableGuidToString());
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comments");

        });

        base.OnModelCreating(modelBuilder);
    }
}


public class IEnumerableGuidToString : ValueConverter<List<string>, string>
{
    public IEnumerableGuidToString() : base (

        list => String.Join(",,,", list.Select(p => p.ToString().ToArray())),
        guidStrings => guidStrings.Split(",,,", StringSplitOptions.None).ToList()
    )
    {

    }
}


