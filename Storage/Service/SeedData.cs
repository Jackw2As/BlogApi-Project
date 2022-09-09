using BlogAPI.Storage.DatabaseModels;
using Faker;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Storage.Service;
public static class SeedData
{
    public static bool IsDatabaseSeeded;
    public static void SeedDatabase(DbContext context)
    {
        //Ensure Database is created before seeding Data
        if (IsDatabaseSeeded) return;
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var seedData = CreateSeedData();
        context.AddRange(seedData);
        context.SaveChanges();
        IsDatabaseSeeded = true;
    }
    private static IEnumerable<object> CreateSeedData()
    {
        var blogs = CreateBlogs().ToArray();
        var posts = CreatPosts(blogs).ToArray();
        var comments = CreateComments(posts);
        
        var collection = new List<object>();
        collection.AddRange(blogs);
        collection.AddRange(posts);
        collection.AddRange(comments);
        return collection;
    }
    public static IEnumerable<Comment> CreateComments(IEnumerable<Post> posts)
    {
        foreach (var post in posts)
        {
            int maxCount = Random.Shared.Next(5);
            for (int count = 0; count < maxCount; count++)
            {
                yield return CreateFakeComment(post);
            }
        }
    }

    private static Comment CreateFakeComment(Post post)
    {
        var rand = Random.Shared;
        TimeSpan dateCreatedOffset = new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60));
        return new Comment
        {
            Username = Internet.UserName(),
            PostId = post.ID,
            Content = Lorem.Paragraph(2),
            DateCreated = post.DateCreated.Add(dateCreatedOffset),
            ID = Guid.NewGuid().ToString()
        };
    }

    public static IEnumerable<Post> CreatPosts(IEnumerable<Blog> blogs)
    {
        foreach (var blog in blogs)
        {
            int maxCount = 2;
            for (int count = 0; count < maxCount; count++)
            {
                yield return CreateFakePost(blog);
            }
        }
    }

    private static Post CreateFakePost(Blog blog)
    {
        Random rand = new Random();
        return new Post()
        {
            Title = Name.First(),
            BlogId = blog.ID,
            Content = Lorem.Paragraph(2),
            DateCreated = new DateTime(2012, new Random().Next(1, 13), new Random().Next(1, 28)),
            DateModified = DateTime.UtcNow
                .Subtract(
                    new TimeSpan(
                        rand.Next(100), 
                        rand.Next(24), 
                        rand.Next(60), 
                        rand.Next(50))
                ),
            ID = Guid.NewGuid().ToString(),
        };
    }

    public static IEnumerable<Blog> CreateBlogs()
    {
        int maxCount = 3;
        for (int count = 0; count < maxCount; count += 1)
        {
            yield return new Blog()
            {
                Name = Name.First(),
                Summary = Lorem.Sentence(1),
                ID = Guid.NewGuid().ToString(),
            };
        }
    }
}
