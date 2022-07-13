using BlogAPI.Storage.DatabaseModels;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Storage.InMemory
{
    public class BaseIntergrationTests
    {
        public WebApplicationFactory<Program> ApplicationFactory { get; init; }
        public BaseIntergrationTests()
        {
            ApplicationFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(
                builder =>
                {
                    
                });
        }
    }

    public class IntergrationTests : BaseIntergrationTests
    {
        public IntergrationTests()
        {
            SeedData();
        }

        [Fact]
        public async void ShouldCreateBlogThenCreatePost()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();
            var server = ApplicationFactory.Server;

            //Create Blog
            var blog = new CreateBlog();
            var blogContent = JsonContent.Create(blog);
            var blogResponse = await client.PostAsync("/blog", blogContent);

            Assert.True(blogResponse.IsSuccessStatusCode);

            //Create Post
            var post = new CreatePost();
            var postContent = JsonContent.Create(post);
            var postResponse = await client.PostAsync("/blog", postContent);

            Assert.True(postResponse.IsSuccessStatusCode);
        }

        [Fact]
        public void ShouldNotCreatePost()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();
            var server = ApplicationFactory.Server;

            //Act
            var post = new CreatePost();
            var postContent = JsonContent.Create(post);
            var postResponse = await client.PostAsync("/blog", postContent);

            //Assert
            Assert.True(postResponse.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData( new object[] { "/blog", GetBlog })]
        [InlineData( new object[] { "/post", GetPost })]
        [InlineData( new object[] { "/comment", GetComment })]
        public async void ShouldGetCorrectContent(string url, Type type)
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();

            //Act
            var response = await client.GetAsync(url);

            //Assert
            Assert.True(response.IsSuccessStatusCode);

            Assert.IsType(type, response.Content);
        }

        [Fact]
        public async void ShouldDeleteBlogAndPostsAndComments()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();
            //Act
            var response = await client.DeleteAsync("/blog");
            //Assert
            Assert.True(response.IsSuccessStatusCode);

            //Assert that no Posts exist that either have a null Blog or point to the blog deleted

            //Assert that no Comments exist that either have a null Post or point to a Post deleted
        }
        [Fact]
        public async void ShouldCreateCommentOnExistingPost()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();
            //Act
            var comment = CreateComment();
            var content = JsonContent.Create(comment);
            var response = await client.PostAsync("/comment", content);
            //Assert
            Assert.True(response.IsSuccessStatusCode);
        }

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
