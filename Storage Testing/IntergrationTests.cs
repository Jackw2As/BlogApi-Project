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

        [Fact]
        public async void ShouldFindABlogThenFindAllPostsForSaidBlog()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();

            //Get Blog
            var blogs = await client.GetFromJsonAsync<List<GetBlog>>("blog/list");
            
            Assert.NotNull(blogs);
            Assert.NotEmpty(blogs);

            var blog = blogs.First();

            //Get Posts
            var posts = await client.GetFromJsonAsync<List<GetBlog>>($"blog/list?ID{blog.ID}");

            Assert.NotNull(posts);
            Assert.NotEmpty(posts);
        }



        [Fact]
        public async void ShouldDeleteBlogAndPostsAndComments()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();
            
            var createBlog = new CreateBlog("Blog Test");
            var postBlogContent = JsonContent.Create(createBlog);
            var blogResponse = await client.PostAsync("/blog",postBlogContent);
            var blogLocation = blogResponse.Headers.Location;
            var blog = await client.GetFromJsonAsync<GetBlog>(blogLocation);

            Assert.NotNull(blog);

            var createPost = new CreatePost("Post Test", "Post Content.", blog);
            var postPostContent = JsonContent.Create(createPost);
            var postResponse = await client.PostAsync("/post", postPostContent);
            var postLocation = postResponse.Headers.Location;
            var post = await client.GetFromJsonAsync<GetPost>(postLocation);

            Assert.NotNull(post);

            var createComment = new CreateComment("Test Username", "Test Comment", post);
            var commentCommentContent = JsonContent.Create(createComment);
            var commentResponse = await client.PostAsync("/comment", commentCommentContent);
            var commentLocation = commentResponse.Headers.Location;
            var comment = await client.GetFromJsonAsync<GetComment>(commentLocation);

            Assert.NotNull(comment);

            //Act
            var response = await client.DeleteAsync($"/blog?ID={blog.ID}");

            //Assert
            Assert.True(response.IsSuccessStatusCode);

            var postList = await client.GetFromJsonAsync<List<GetPost>>($"post/list?ID={blog.ID}");
            var commentList = await client.GetFromJsonAsync<List<GetComment>>($"post/list?ID={blog.ID}");

            Assert.Empty(postList);
            Assert.Empty(commentList);
        }
    }
}
