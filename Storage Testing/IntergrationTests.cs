using BlogAPI.Application.ApiModels;
using BlogAPI.Storage.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

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
        }

        [Fact]
        public async void ShouldCreateBlogThenCreatePost()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();

            //Create Blog
            var createBlog = new CreateBlog(Faker.Name.First());
            var blogContent = JsonContent.Create(createBlog);
            var blogResponse = await client.PostAsync("/blog", blogContent);

            Assert.True(blogResponse.IsSuccessStatusCode);

            var blogLocation = blogResponse.Headers.Location;

            Assert.NotNull(blogLocation);

            //Create Post
            var blog = await client.GetFromJsonAsync<GetBlog>(blogLocation);

            Assert.NotNull(blog);
            var post = new CreatePost(Faker.Lorem.GetFirstWord(), Faker.Lorem.Paragraph(10), blog!);
            var postContent = JsonContent.Create(post);
            var postResponse = await client.PostAsync("/post", postContent);

            Assert.True(postResponse.IsSuccessStatusCode);
        }

        [Fact]
        public async void ShouldNotCreatePost()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();

            //Act
            string GetBlog = "";
            var post = new CreatePost("test", "test content", GetBlog!);
            var postContent = JsonContent.Create(post);
            var postResponse = await client.PostAsync("/blog", postContent);

            //Assert
            Assert.False(postResponse.IsSuccessStatusCode);
        }

        /*
        [Theory]
        [InlineData( new object[] { "/blog", Blog })]
        [InlineData( new object[] { "/post", Post })]
        [InlineData( new object[] { "/comment", Comment })]
        public async void ShouldGetCorrectContent(string url, Type type)
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();

            //Act
            var response = await client.GetAsync(url);

            //Assert
            Assert.True(response.IsSuccessStatusCode);

            Assert.IsType(type, await response.Content.ReadFromJsonAsync(type);
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
        */
    }
}
