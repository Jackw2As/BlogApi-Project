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
            var posts = await client.GetFromJsonAsync<List<GetPost>>($"post/list?BlogId={blog.ID}");

            Assert.NotNull(posts);
            Assert.NotEmpty(posts);
        }

        [Fact]
        public async void ShouldPostACommentOnAnExistingBlog()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();

            //Get Blog
            var blogs = await client.GetFromJsonAsync<List<GetBlog>>("blog/list");

            Assert.NotNull(blogs);
            Assert.NotEmpty(blogs);

            var blog = blogs.First();

            //Get Posts
            var posts = await client.GetFromJsonAsync<List<GetPost>>($"post/list?BlogId={blog.ID}");

            Assert.NotNull(posts);
            Assert.NotEmpty(posts);

            var post = posts.First();

            //Act

            //Create Comment
            var createComment = new CreateComment(Faker.Name.First(), Faker.Lorem.Sentence(), post);
            var commentContent = JsonContent.Create(createComment);
            var commentResponse = await client.PostAsync("/comment", commentContent);

            //Assert
            //is Created
            Assert.True(commentResponse.IsSuccessStatusCode);
            
            var commentLocation = commentResponse.Headers.Location;
            Assert.NotNull(commentLocation);
            
            //returns expected values
            var comment = await client.GetFromJsonAsync<GetComment>(commentLocation);
            Assert.NotNull(comment);
            Assert.Equal(createComment.Username, comment.Username);
            Assert.Equal(createComment.Content, comment.Content);
            Assert.Equal(createComment.PostId, comment.Post.ID);
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

            var blogList = await client.GetFromJsonAsync<List<GetBlog>>($"blog/list");
            var postList = await client.GetFromJsonAsync<List<GetPost>>($"post/list?ID={blog.ID}");
            var commentList = await client.GetFromJsonAsync<List<GetComment>>($"post/list?ID={post.ID}");

            Assert.DoesNotContain(blog, blogList);
            Assert.Empty(postList);
            Assert.Empty(commentList);
        }


        [Fact]
        public async void ShouldDeletePostAndComments()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();

            var createBlog = new CreateBlog("Blog Test");
            var postBlogContent = JsonContent.Create(createBlog);
            var blogResponse = await client.PostAsync("/blog", postBlogContent);
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
            var response = await client.DeleteAsync($"/post?ID={blog.ID}");

            //Assert
            Assert.True(response.IsSuccessStatusCode);

            var blogList = await client.GetFromJsonAsync<List<GetBlog>>($"blog/list");
            var postList = await client.GetFromJsonAsync<List<GetPost>>($"post/list?ID={blog.ID}");
            var commentList = await client.GetFromJsonAsync<List<GetComment>>($"post/list?ID={post.ID}");

            Assert.Contains(blog, blogList);
            Assert.DoesNotContain(post, postList);
            Assert.Empty(commentList);
        }

        [Fact]
        public async void ShouldDeleteComment()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();

            var createBlog = new CreateBlog("Blog Test");
            var postBlogContent = JsonContent.Create(createBlog);
            var blogResponse = await client.PostAsync("/blog", postBlogContent);
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
            var response = await client.DeleteAsync($"/comment?id={comment.ID}");

            //Assert
            Assert.True(response.IsSuccessStatusCode);

            var blogList = await client.GetFromJsonAsync<List<GetBlog>>($"blog/list");
            var postList = await client.GetFromJsonAsync<List<GetPost>>($"post/list?ID={blog.ID}");
            var commentList = await client.GetFromJsonAsync<List<GetComment>>($"post/list?ID={post.ID}");

            Assert.Contains(blog, blogList);
            Assert.Contains(post, postList);
            Assert.DoesNotContain(comment, commentList);
        }
    }
}
