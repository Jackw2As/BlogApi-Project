using BlogAPI.Application.ApiModels;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace BlogAPI.Application;

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
    #region Helper Methods
    protected static async Task<GetComment> CreateComment(HttpClient client, GetPost post)
    {
        var createComment = new CreateComment("Test Username", "Test Comment", post);
        var commentCommentContent = JsonContent.Create(createComment);
        var commentResponse = await client.PostAsync("/comment", commentCommentContent);
        var commentLocation = commentResponse.Headers.Location;
        var comment = await client.GetFromJsonAsync<GetComment>(commentLocation);
        return comment;
    }

    protected static async Task<GetComment> CreateComment(HttpClient client)
    {
        var blog = await CreateBlog(client);
        var post = await CreatePost(client, blog);
        return await CreateComment(client, post);
    }

    protected static async Task<GetPost> CreatePost(HttpClient client, GetBlog blog)
    {
        var createPost = new CreatePost("Post Test", "Post Content.", blog);
        var postContent = JsonContent.Create(createPost);
        var postResponse = await client.PostAsync("/post", postContent);
        var postLocation = postResponse.Headers.Location;
        var post = await client.GetFromJsonAsync<GetPost>(postLocation);
        return post;
    }

    protected static async Task<GetPost> CreatePost(HttpClient client)
    {
        var blog = await CreateBlog(client);
        return await CreatePost(client, blog);
    }

    protected static async Task<GetBlog> CreateBlog(HttpClient client)
    {
        var createBlog = new CreateBlog("Blog Test");
        var postBlogContent = JsonContent.Create(createBlog);
        var blogResponse = await client.PostAsync("/blog", postBlogContent);
        var blogLocation = blogResponse.Headers.Location;
        var blog = await client.GetFromJsonAsync<GetBlog>(blogLocation);
        return blog;
    }

    protected static async Task<HttpResponseMessage> CreateBlog(HttpClient client, CreateBlog createBlog)
    {
        var postBlogContent = JsonContent.Create(createBlog);
        return await client.PostAsync("/blog", postBlogContent);
    }

    protected static async Task<HttpResponseMessage> CreatePost(HttpClient client, CreatePost createPost)
    {
        var postContent = JsonContent.Create(createPost);
        return await client.PostAsync("/post", postContent);
    }

    protected static async Task<HttpResponseMessage> CreateComment(HttpClient client, CreateComment createComment)
    {
        var commentCommentContent = JsonContent.Create(createComment);
        return await client.PostAsync("/comment", commentCommentContent);
    }

    protected static async Task<HttpResponseMessage> ModifyBlog(HttpClient client, ModifyBlog createBlog)
    {
        var postBlogContent = JsonContent.Create(createBlog);
        return await client.PostAsync("/blog/update", postBlogContent);
    }

    protected static async Task<HttpResponseMessage> ModifyPost(HttpClient client, ModifyPost createPost)
    {
        var postContent = JsonContent.Create(createPost);
        return await client.PostAsync("/post/update", postContent);
    }

    protected static async Task<HttpResponseMessage> ModifyComment(HttpClient client, ModifyComment createComment)
    {
        var commentCommentContent = JsonContent.Create(createComment);
        return await client.PostAsync("/comment/update", commentCommentContent);
    }
    #endregion
}
