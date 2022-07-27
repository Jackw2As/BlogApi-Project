using BlogAPI.Application.ApiModels;
using BlogAPI.Application.TestData;
using BlogAPI.Storage.InMemory;

namespace BlogAPI.Application;

public class ModelValidationTests : BaseIntergrationTests
{
    #region Blog Models Tests
    [Fact]
    public async void BlogCreateShouldSucceedValidation()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        //Act
        var blog = await CreateBlog(client);

        //Assert
        Assert.NotNull(blog);
    }

    [Theory]
    [ClassData(typeof(BlogCreateTestData))]
    public async void BlogCreateShouldFailValidation(CreateBlog invalidBlog)
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        //Act
        var blog = await CreateBlog(client, invalidBlog);

        //Assert
        Assert.False(blog.IsSuccessStatusCode);
    }

    [Fact]
    public async void BlogShouldModifySuccessfully()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();
        var blog = await CreateBlog(client);
        Assert.NotNull(blog);
        //Act
        var modifiedBlog = new ModifyBlog(blog);
        modifiedBlog.Name = "A Correctly Named Blog";
        modifiedBlog.Summary = "A Correctly Named Summary";

        var response = await ModifyBlog(client, modifiedBlog);

        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [ClassData(typeof(BlogModifyTestData))]
    public async void BlogModifyShouldFailValidation(ModifyBlog invalidBlog)
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();
        var blog = await CreateBlog(client);
        Assert.NotNull(blog);
        invalidBlog.ID = blog.ID;

        //Act
        var response = await ModifyBlog(client, invalidBlog);

        //Assert
        Assert.False(response.IsSuccessStatusCode);
    }
    #endregion

    #region Post Models Tests
    [Fact]
    public async void PostCreateShouldSucceedValidation()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        //Act
        var post = await CreatePost(client);

        //Assert
        Assert.NotNull(post);
    }

    [Theory]
    [ClassData(typeof(PostCreateTestData))]
    public async void PostCreateShouldFailValidation(CreatePost invalidPost)
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        //Act
        var response = await CreatePost(client, invalidPost);

        //Assert
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    //Blog can not be null.
    public async void PostCreateShouldFailValidation2()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        //Act
        var invalidPost = new CreatePost();

        invalidPost.Title = "Good Title";
        invalidPost.BlogID = null;

        var post = await CreatePost(client, invalidPost);
        //Assert
        Assert.False(post.IsSuccessStatusCode);
    }
    [Fact]
    //Blog can not be invalid
    public async void PostCreateShouldFailValidation3()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        //Act
        var invalidPost = new CreatePost();

        invalidPost.Title = "Good Title";
        invalidPost.BlogID = Guid.NewGuid().ToString();

        var post = await CreatePost(client, invalidPost);
        //Assert
        Assert.False(post.IsSuccessStatusCode);
    }

    [Fact]
    public async void PostShouldModifySuccessfully()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();
        var post = await CreatePost(client);
        Assert.NotNull(post);
        //Act
        var modifiedPost = new ModifyPost(post);
        modifiedPost.Summary = "Correct Summary";
        modifiedPost.Content = "Correct Content";
        modifiedPost.Title = "Correct Title";

        var response = await ModifyPost(client, modifiedPost);

        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [ClassData(typeof(PostModifyTestData))]
    public async void PostModifyShouldFail(ModifyPost invalidPost)
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();
        var post = await CreatePost(client);
        Assert.NotNull(post);

        //invalidComment shouldn't fail here because ID is invalid.
        invalidPost.ID = post.ID;

        //Act
        var response = await ModifyPost(client, invalidPost);
        //Assert
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    //Using Incorrect ID
    public async void PostModifyShouldFail2()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        //Act
        var invalidPost = new ModifyPost("My Title", "Fantastic Content");
        invalidPost.ID = Guid.NewGuid().ToString();

        var post = await ModifyPost(client, invalidPost);
        //Assert
        Assert.False(post.IsSuccessStatusCode);
    }

    [Fact]
    //ID can't be null
    public async void PostModifyShouldFail3()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        //Act
        var invalidPost = new ModifyPost("My Title", "Fantastic Content");
        invalidPost.ID = null;

        var response = await ModifyPost(client, invalidPost);
        //Assert
        Assert.False(response.IsSuccessStatusCode);
    }
    #endregion

    #region Comment Models Tests
    [Fact]
    public async void CommentCreateShouldSucceedValidation()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        //Act
        var comment = await CreateComment(client);

        //Assert
        Assert.NotNull(comment);
    }

    [Theory]
    [ClassData(typeof(CommentCreateTestData))]
    public async void CommentCreateShouldFailValidation(CreateComment invalidComment)
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        //Act
        var post = await CreatePost(client);
        invalidComment.PostId = post.ID;

        var response = await CreateComment(client, invalidComment);

        //Assert
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    //Using Incorrect PostID
    public async void CommentCreateShouldFailValidation2()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        //Act
        var invalidComment = new CreateComment("username", "Great Post", Guid.NewGuid().ToString());

        var post = await CreateComment(client, invalidComment);
        //Assert
        Assert.False(post.IsSuccessStatusCode);
    }

    [Fact]
    //PostID can't be null
    public async void CommentCreateShouldFailValidation3()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        //Act
        var invalidComment = new CreateComment("username", "Great Post", (GetPost)null);

        var post = await CreateComment(client, invalidComment);
        //Assert
        Assert.False(post.IsSuccessStatusCode);
    }

    [Fact]
    public async void CommentShouldModifySuccessfully()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();
        var comment = await CreateComment(client);
        Assert.NotNull(comment);
        //Act
        var modifiedComment = new ModifyComment(comment);
        modifiedComment.Content = "A Correctly Named Comment";

        var response = await ModifyComment(client, modifiedComment);

        Assert.True(response.IsSuccessStatusCode);
    }
    #endregion

    [Theory]
    [ClassData(typeof(CommentModifyTestData))]
    public async void CommentModifyShouldFailValidation(ModifyComment invalidComment)
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();
        var comment = await CreateComment(client);
        Assert.NotNull(comment);

        //invalidComment shouldn't fail here because ID is invalid.
        invalidComment.ID = comment.ID;

        //Act
        var response = await ModifyComment(client, invalidComment);
        //Assert
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    //Using Incorrect ID
    public async void CommentModifyShouldFailValidation2()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        //Act
        var invalidComment = new ModifyComment("Great Post");
        invalidComment.ID = Guid.NewGuid().ToString();

        var response = await ModifyComment(client, invalidComment);
        //Assert
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    //ID can't be null
    public async void CommentModifyShouldFailValidation3()
    {
        //Arrange
        var client = ApplicationFactory.CreateDefaultClient();

        //Act
        var invalidComment = new ModifyComment("Great Post");
        invalidComment.ID = null;

        var response = await ModifyComment(client, invalidComment);
        //Assert
        Assert.False(response.IsSuccessStatusCode);
    }
}
