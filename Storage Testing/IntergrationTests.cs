using BlogAPI.Storage.DatabaseModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Storage.InMemory
{
    public class IntergrationTests
    {
        private SqliteConnection _connection;
        private readonly DbContextOptions _contextOptions;

        protected IEnumerable<Comment> MockComments;
        protected IEnumerable<Blog> MockBlogs;
        protected IEnumerable<Post> MockPosts;

        public IntergrationTests()
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<InMemoryDBContext>()
                .UseSqlite(_connection)
                .Options;

            SeedData();
        }

        public void Dispose() => _connection.Dispose();

        protected InMemoryDBContext CreateContext => new(_contextOptions);
        protected InMemoryRepository<Blog> CreateBlogRepository() => new InMemoryRepository<Blog>(CreateContext);
        protected InMemoryRepository<Post> CreatePostRepository() => new InMemoryRepository<Post>(CreateContext);
        protected InMemoryRepository<Comment> CreateCommentRepository() => new InMemoryRepository<Comment>(CreateContext);

        #region Blog Tests

        [Fact]
        public void ShouldCreateBlog()
        {
            //Arrange
            var repository = CreateBlogRepository();
            var item = MockBlogs.First();

            //Act
            var blog = repository.Save(item);

            //Assert

            throw new NotImplementedException();
        }

        [Fact]
        public void ShouldGetBlog()
        {
            //Arrange
            //Act
            //Assert
            throw new NotImplementedException();
        }

        [Fact]
        public void ShouldEditBlog()
        {
            //Arrange
            //Act
            //Assert
            throw new NotImplementedException();
        }

        [Fact]
        public void ShouldDeleteBlog()
        {
            //Arrange
            //Act
            //Assert
            throw new NotImplementedException();
        }

        #endregion

        #region Post Tests

        #endregion

        #region Comment Tests

        #endregion


        #region Helper
        private void SeedData()
        {
            using var context = new InMemoryDBContext(_contextOptions);

            if (context.Database.EnsureCreated())
            {
                context.AddRange(CreateMockObjects());

                context.SaveChanges();
            }
        }

        private object[] CreateMockObjects()
        {
            var collection = new List<object>();

            collection.AddRange(CreateMockBlogs());
            collection.AddRange(CreateMockPosts());
            collection.AddRange(CreateMockComments());

            return collection.ToArray();
        }

        private IEnumerable<Comment> CreateMockComments()
        {
            var collection = new List<Comment>();
            var rand = new Random();

            foreach (var post in MockPosts)
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

            MockComments = collection;
            return collection;
        }

        private IEnumerable<Post> CreateMockPosts()
        {
            var collection = new List<Post>();

            foreach(var blog in MockBlogs)
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
            
            MockPosts = collection;
            return collection;
        }

        private IEnumerable<Blog> CreateMockBlogs()
        {
            var collection = new List<Blog>();

            int maxCount = 3;
            int count = 0;
            while(count < maxCount)
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

            MockBlogs = collection;
            return collection;
        }
        #endregion
    }
}
