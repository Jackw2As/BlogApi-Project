﻿using BlogAPI.Application.ApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace BlogAPI.Storage.InMemory
{
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
            await Assert.ThrowsAsync<HttpRequestException>(async() => await client.GetFromJsonAsync<List<GetComment>>($"comment/list?PostId={post.ID}"));

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

        [Fact]
        //Name Minimuim Length 4
        public async void BlogCreateShouldFailValidation()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();

            //Act
            var invalidBlog = new CreateBlog();

            invalidBlog.Name = "123";
            var blog = await CreateBlog(client, invalidBlog);
            //Assert
            Assert.False(blog.IsSuccessStatusCode);
        }

        [Fact]
        //Name Maximum Length 24
        public async void BlogCreateShouldFailValidation2()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();

            //Act
            var invalidBlog = new CreateBlog();

            invalidBlog.Name = Faker.Lorem.Sentence(25);
            var blog = await CreateBlog(client, invalidBlog);
            //Assert
            Assert.False(blog.IsSuccessStatusCode);
        }

        [Fact]
        //Summary Maximum Length 300
        public async void BlogCreateShouldFailValidation3()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();

            //Act
            var invalidBlog = new CreateBlog();

            invalidBlog.Name = "Good Name";
            invalidBlog.Summary = Faker.Lorem.Sentence(301);
            var blog = await CreateBlog(client, invalidBlog);
            //Assert
            Assert.False(blog.IsSuccessStatusCode);
        }

        [Fact]
        //Summary Minimum Length 1
        public async void BlogCreateShouldFailValidation4()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();

            //Act
            var invalidBlog = new CreateBlog();

            invalidBlog.Name = "Good Name";
            invalidBlog.Summary = "";
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

        [Fact]
        //Name too short (min 4)
        public async void BlogModifyShouldFailValidation()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();
            var blog = await CreateBlog(client);
            Assert.NotNull(blog);

            //Act
            var invalidBlog = new CreateBlog();
            invalidBlog.Name = "123";
            var response = await CreateBlog(client, invalidBlog);
            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }
        [Fact]
        //Name too long (max 24)
        public async void BlogModifyShouldFailValidation2()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();
            var blog = await CreateBlog(client);
            Assert.NotNull(blog);

            //Act
            var invalidBlog = new CreateBlog();
            invalidBlog.Name = Faker.Lorem.Sentence(25);
            var response = await CreateBlog(client, invalidBlog);
            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }
        [Fact]
        //Summary Minimum Length 1
        public async void BlogModifyShouldFailValidation3()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();
            var blog = await CreateBlog(client);
            Assert.NotNull(blog);

            //Act
            var invalidBlog = new CreateBlog();
            invalidBlog.Name = "Good Name";
            invalidBlog.Summary = "";
            var response = await CreateBlog(client, invalidBlog);
            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }
        [Fact]
        //Summary too long(max length 300)
        public async void BlogModifyShouldFailValidation4()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();
            var blog = await CreateBlog(client);
            Assert.NotNull(blog);

            //Act
            var invalidBlog = new CreateBlog();
            invalidBlog.Name = "Good Name";
            invalidBlog.Summary = Faker.Lorem.Sentence(301); ;
            var response = await CreateBlog(client, invalidBlog);
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

        [Fact]
        //Title Minimuim Length 3
        public async void PostCreateShouldFailValidation()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();

            //Act
            var invalidPost = new CreatePost();

            invalidPost.Title = "12";
            var post = await CreatePost(client, invalidPost);
            //Assert
            Assert.False(post.IsSuccessStatusCode);
        }

        [Fact]
        //Title Max Length 100
        public async void PostCreateShouldFailValidation2()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();

            //Act
            var invalidPost = new CreatePost();

            invalidPost.Title = Faker.Lorem.Sentence(101);
            var post = await CreatePost(client, invalidPost);
            //Assert
            Assert.False(post.IsSuccessStatusCode);
        }
        [Fact]
        //Summary Min Length 1
        public async void PostCreateShouldFailValidation3()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();

            //Act
            var invalidPost = new CreatePost();

            invalidPost.Title = "Good Title";
            invalidPost.Summary = "";
            var post = await CreatePost(client, invalidPost);

            //Assert
            Assert.False(post.IsSuccessStatusCode);
        }
        [Fact]
        //Summary Max Length 255
        public async void PostCreateShouldFailValidation4()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();

            //Act
            var invalidPost = new CreatePost();

            invalidPost.Title = "Good Title";
            invalidPost.Summary = Faker.Lorem.Sentence(256);

            var post = await CreatePost(client, invalidPost);
            //Assert
            Assert.False(post.IsSuccessStatusCode);
        }
        [Fact]
        //Content can't be null
        public async void PostCreateShouldFailValidation5()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();

            //Act
            var invalidPost = new CreatePost();

            invalidPost.Title = "Good Title";
            invalidPost.Content = null;

            var post = await CreatePost(client, invalidPost);
            //Assert
            Assert.False(post.IsSuccessStatusCode);
        }

        [Fact]
        //Content Max Length 5000
        public async void PostCreateShouldFailValidation6()
        {
            //Arrange
            var client = ApplicationFactory.CreateDefaultClient();

            //Act
            var invalidPost = new CreatePost();

            invalidPost.Title = "Good Title";
            invalidPost.Content = Faker.Lorem.Sentence(5001);

            var post = await CreatePost(client, invalidPost);
            //Assert
            Assert.False(post.IsSuccessStatusCode);
        }

        [Fact]
        //Blog can not be null.
        public async void PostCreateShouldFailValidation7()
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
        public async void PostCreateShouldFailValidation8()
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

    internal class PostModifyTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            //Title can't be null
            yield return new object[] { new ModifyPost(null, "some content") };

            //Title min 3
            yield return new object[] { new ModifyPost("12", "some content") };

            //Title max 100
            yield return new object[] { new ModifyPost(Faker.Lorem.Sentence(101), "some content") };

            //Summary max 255
            yield return new object[] { new ModifyPost("Great Title", "some content", Faker.Lorem.Sentence(256)) };

            //Content can't be null
            yield return new object[] { new ModifyPost("Great Title", null) };

            //Content max 5000
            yield return new object[] { new ModifyPost("Great Title", Faker.Lorem.Sentence(5000)) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class CommentCreateTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            //Username can't be null
            yield return new object[] { new CreateComment(null, string.Empty, string.Empty) };

            //Username max length 20
            yield return new object[] { new CreateComment(Faker.Lorem.Sentence(21), string.Empty, string.Empty) };

            //Username should not contain whitespace.
            yield return new object[] { new CreateComment("Invalid User Name", null, string.Empty) };

            //Content can't be null
            yield return new object[] { new CreateComment("ValidUserName", null, string.Empty) };

            //Content max length is 300
            yield return new object[] { new CreateComment("ValidUserName", Faker.Lorem.Sentence(301), string.Empty) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class CommentModifyTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            //Content can't be null
            yield return new object[] { new ModifyComment((string)null) };

            //Content max length is 300
            yield return new object[] { new ModifyComment(Faker.Lorem.Sentence(301)) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
