using BlogAPI.Storage.DatabaseModels;
using BlogAPI.Storage.InMemory;

public static class SeedData
{
    public static void SeedDatabase(InMemoryDBContext context)
    {
        if (context.Database.EnsureCreated())
        {
            var seedData = CreateSeedData();
            context.AddRange(seedData);
            context.SaveChanges();
        }
    }
    private static object[] CreateSeedData()
    {
        var collection = new List<object>();

        var blogs = CreateBlogs();
        var posts = CreatPosts(blogs.ToList());
        var comments = CreateComments(posts.ToList());

        collection.AddRange(blogs);
        collection.AddRange(posts);
        collection.AddRange(comments);

        return collection.ToArray();
    }

    private static IEnumerable<Comment> CreateComments(List<Post> posts)
    {
        var collection = new List<Comment>();
        var rand = new Random();

        foreach (var post in posts)
        {
            int maxCount = rand.Next(5);
            int count = 0;
            while (count < maxCount)
            {
                var DateCreatedOffset = new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60));

                var comment = new Comment()
                {
                    Username = Faker.Internet.UserName(),
                    Post = post,
                    Content = Faker.Lorem.Paragraph(2),
                    DateCreated = post.DateCreated.Add(DateCreatedOffset)
                };

                collection.Add(comment);
                post.Comments.Add(comment);
                count += 1;
            }
        }
        return collection;
    }

    private static IEnumerable<Post> CreatPosts(List<Blog> blogs)
    {
        var collection = new List<Post>();

        foreach (var blog in blogs)
        {
            int maxCount = 2;
            int count = 0;
            while (count < maxCount)
            {
                var post = new Post()
                {
                    Name = Faker.Name.First(),
                    Blog = blog,
                    Content = Faker.Lorem.Paragraph(2),
                    DateCreated = new DateTime(2012, new Random().Next(1, 13), new Random().Next(1, 28)),
                    DateModified = DateTime.UtcNow,
                    Comments = new()
                };

                var rand = new Random();
                post.DateModified.Subtract(new TimeSpan(rand.Next(100), rand.Next(24), rand.Next(60), rand.Next(50)));

                collection.Add(post);
                blog.Posts.Add(post);
                count += 1;
            }
        }
        return collection;
    }

    private static IEnumerable<Blog> CreateBlogs()
    {
        var collection = new List<Blog>();

        int maxCount = 3;
        int count = 0;
        while (count < maxCount)
        {
            var blog = new Blog()
            {
                Name = Faker.Name.First(),
                Summary = Faker.Lorem.Sentence(1),
                Posts = new List<Post>()
            };
            collection.Add(blog);
            count += 1;
        }

        return collection;
    }
}
