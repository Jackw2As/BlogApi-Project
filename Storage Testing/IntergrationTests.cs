using BlogAPI.Application.ApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace BlogAPI.Application;

public class IntergrationTests : BaseIntergrationTests
{
    public IntergrationTests()
    {
    }

    #region Create Tests
    [Fact]
    public async void ShouldCreateBlogThenCreatePost()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        //Create Blog
        var blog = await CreateBlog(client);

        //Create Post
        Assert.NotNull(blog);
        var post = await CreatePost(client, blog);

        Assert.NotNull(post);
        var posts = await client.GetFromJsonAsync<List<GetPost>>($"/post/list?BlogID={blog.ID}");
        Assert.Contains(posts, p => p.ID == post.ID);
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
    public async void ShouldPostACommentOnAnExistingPost()
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
    }

    [Fact]
    public async void ShouldNotCreatePost()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        //Act
        string invalidId = "";
        var post = new CreatePost("test", "test content", invalidId!);
        var postContent = JsonContent.Create(post);
        var postResponse = await client.PostAsync("/post", postContent);

        //Assert
        Assert.False(postResponse.IsSuccessStatusCode);
    }

    [Fact]
    public async void ShouldNotCreateComment()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        //Act
        string invalidId = "";
        var post = new CreateComment("test", "test content", invalidId!);
        var postContent = JsonContent.Create(post);
        var postResponse = await client.PostAsync("/comment", postContent);

        //Assert
        Assert.False(postResponse.IsSuccessStatusCode);
    }
    #endregion

    #region Delete Tests
    [Fact]
    public async void ShouldDeleteBlogAndPostsAndComments()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        var blog = await CreateBlog(client);
        Assert.NotNull(blog);

        var post = await CreatePost(client, blog);
        Assert.NotNull(post);

        var comment = await CreateComment(client, post);
        Assert.NotNull(comment);

        //Act
        var response = await client.DeleteAsync($"/blog?ID={blog.ID}");

        //Assert
        Assert.True(response.IsSuccessStatusCode);

        var blogList = await client.GetFromJsonAsync<List<GetBlog>>($"blog/list");
        await Assert.ThrowsAsync<HttpRequestException>(async () => await client.GetFromJsonAsync<List<GetPost>>($"post/list?BlogId={blog.ID}"));
        await Assert.ThrowsAsync<HttpRequestException>(async () => await client.GetFromJsonAsync<List<GetComment>>($"comment/list?PostId={post.ID}"));

        Assert.DoesNotContain(blog, blogList);
    }

    [Fact]
    public async void ShouldDeletePostAndComments()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();
        GetBlog? blog = await CreateBlog(client);

        Assert.NotNull(blog);
        GetPost? post = await CreatePost(client, blog);

        Assert.NotNull(post);
        GetComment? comment = await CreateComment(client, post);

        Assert.NotNull(comment);

        //Act
        var response = await client.DeleteAsync($"/post?id={post.ID}");

        //Assert
        Assert.True(response.IsSuccessStatusCode);

        var blogList = await client.GetFromJsonAsync<List<GetBlog>>($"blog/list");
        var postList = await client.GetFromJsonAsync<List<GetPost>>($"post/list?BlogId={blog.ID}");
        await Assert.ThrowsAsync<HttpRequestException>(async () => await client.GetFromJsonAsync<List<GetComment>>($"comment/list?PostId={post.ID}"));

        Assert.Contains(blog, blogList);
        Assert.DoesNotContain(post, postList);
    }

    [Fact]
    public async void ShouldDeleteComment()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        var blog = await CreateBlog(client);
        Assert.NotNull(blog);

        var post = await CreatePost(client, blog);
        Assert.NotNull(post);

        var comment = await CreateComment(client, post);
        Assert.NotNull(comment);

        //Act
        var response = await client.DeleteAsync($"/comment?id={comment.ID}");

        //Assert
        Assert.True(response.IsSuccessStatusCode);

        var blogList = await client.GetFromJsonAsync<List<GetBlog>>($"blog/list");
        var postList = await client.GetFromJsonAsync<List<GetPost>>($"post/list?BlogId={blog.ID}");
        var commentList = await client.GetFromJsonAsync<List<GetComment>>($"comment/list?PostId={post.ID}");

        Assert.Contains(blog, blogList);
        Assert.Contains(post, postList);
        Assert.DoesNotContain(comment, commentList);
    }

    #endregion

    #region Modify Tests
    [Fact]
    public async void ModifyCommentSuccessfully()
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

        //Get Comment
        var comments = await client.GetFromJsonAsync<List<GetComment>>($"comment/list?PostId={post.ID}");
        Assert.NotNull(comments);


        Assert.NotEmpty(comments);

        //Act
        GetComment getComment = new();
        //Sometimes default database seeding doesn't create a comment.
        if (comments.Count < 1)
        {
            getComment = await CreateComment(client, post);
        }
        else
        {
            getComment = comments.First();
        }

        Assert.NotNull(getComment);

        //Modify Commment
        var modifyComment = new ModifyComment(getComment);

        modifyComment.Content = "new content";

        var commentContent = JsonContent.Create(modifyComment);
        var commentResponse = await client.PostAsync("comment/update", commentContent);


        Assert.True(commentResponse.IsSuccessStatusCode);
        var result = await client.GetFromJsonAsync<GetComment>($"comment?Id={modifyComment.ID}");
        Assert.Equal(getComment.ID, result.ID);
        Assert.Equal(getComment.Username, result.Username);
        Assert.NotEqual(getComment.Content, result.Content);
        Assert.Equal(modifyComment.Content, result.Content);
    }
    [Fact]
    public async void ModifyPostSuccessfully()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        //Get Blog
        var blogs = await client.GetFromJsonAsync<List<GetBlog>>("blog/list");

        Assert.NotNull(blogs);
        Assert.NotEmpty(blogs);

        var blog = blogs.First();

        //Act
        //Get Posts
        var posts = await client.GetFromJsonAsync<List<GetPost>>($"post/list?BlogId={blog.ID}");

        Assert.NotNull(posts);
        Assert.NotEmpty(posts);

        var getPost = posts.First();
        Assert.NotNull(getPost);

        //Modify Post
        var modifyPost = new ModifyPost(getPost);

        modifyPost.Title = "New Title";
        modifyPost.Summary = "new summary for content";
        modifyPost.Content = "new content isn't it great?";

        var postContent = JsonContent.Create(modifyPost);
        var postResponse = await client.PostAsync("post/update", postContent);

        //Assert
        Assert.True(postResponse.IsSuccessStatusCode);
        var result = await client.GetFromJsonAsync<GetPost>($"post?Id={modifyPost.ID}");
        Assert.Equal(getPost.ID, result.ID);

        Assert.NotEqual(getPost.Summary, result.Summary);
        Assert.NotEqual(getPost.Title, result.Title);
        Assert.NotEqual(getPost.Content, result.Content);

        Assert.Equal(modifyPost.Summary, result.Summary);
        Assert.Equal(modifyPost.Title, result.Title);
        Assert.Equal(modifyPost.Content, result.Content);
    }
    [Fact]
    public async void ModifyBlogSuccessfully()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        //Get Blog
        var blogs = await client.GetFromJsonAsync<List<GetBlog>>("blog/list");

        Assert.NotNull(blogs);
        Assert.NotEmpty(blogs);

        var getBlog = blogs.First();

        //Modify Blog
        var modifyBlog = new ModifyBlog(getBlog);

        modifyBlog.Name = "New Name";
        modifyBlog.Summary = "New Summary!";

        var blogContent = JsonContent.Create(modifyBlog);
        var PostResponse = await client.PostAsync("blog/update", blogContent);

        //Assert
        Assert.True(PostResponse.IsSuccessStatusCode);
        var result = await client.GetFromJsonAsync<GetBlog>($"blog?Id={modifyBlog.ID}");
        Assert.Equal(getBlog.ID, result.ID);

        Assert.NotEqual(getBlog.Name, result.Name);
        Assert.NotEqual(getBlog.Summary, result.Summary);

        Assert.Equal(modifyBlog.Name, result.Name);
        Assert.Equal(modifyBlog.Summary, result.Summary);
    }
    #endregion


}
