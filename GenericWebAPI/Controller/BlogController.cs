﻿using BlogAPI.Application.ApiModels;
using BlogAPI.Storage.DatabaseModels;
using Domain.ActionResults;
using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Application.Controller;

public class BlogController : BaseController<Blog>
{
    public IRepository<Post> PostRepository { get; }
    public IRepository<Comment> CommentRepository { get; }
    public BlogController(IRepository<Blog> repository, IRepository<Post> postRepository, IRepository<Comment> commentRepository) : base(repository)
    {
        PostRepository = postRepository;
        CommentRepository = commentRepository;
    }

    [HttpPost]
    public ActionResult<Blog> Post([FromBody]CreateBlog model)
    {
        var blog = new Blog()
        {
            ID = Guid.NewGuid().ToString(),
            Name = model.Name,
            Summary = model.Summary ?? "My Fantastic New Blog!!!",
        };

        return base.Post(blog);
    }

    [HttpGet]
    public ActionResult<GetBlog> GetById([FromQuery]string id)
    {
        var result = base.GetById(id);
        var item = (result.Result as ObjectResult).Value as Blog;

        if (item == null)
        {
            return new NotFoundObjectResult(item);
        }

        return new ObjectResult(new GetBlog(){
            ID = item.ID,
            Name = item.Name,
            Summary = item.Summary,
        });
    }

    [HttpGet("List")]
    public ActionResult<List<GetBlog>> GetAll()
    {
        var blogs = Repository.GetByQuery(blog => true);

        List<GetBlog> getBlogs = new List<GetBlog>();
        foreach (var blog in blogs)
        {
            getBlogs.Add(new(blog));
        }
        return new ObjectResult(getBlogs);
    }

    public ActionResult Delete([FromQuery] Guid id)
    {
        if (!Repository.Exists(id.ToString()))
        {
            ModelState.AddModelError(nameof(id), "id is invalid");
            return new BadRequestObjectResult(ModelState);
        }

        var blog = Repository.GetByID(id.ToString());

        //Delete Posts
        foreach (var postId in blog.PostIds)
        {
            var post = PostRepository.GetByID(postId.ToString());

            //Delete Comments
            foreach (var comment in PostRepository.GetByID(postId).CommentIds)
            {
                var result1 = CommentRepository.Delete(comment);
                if (!result1)
                {
                    //Save any changes made(aka removed comments deleted)
                    PostRepository.Modify(post);
                    return new ServerError($"Unable to delete comment id = {comment}. Try deleting comment before deleting blog!");
                }
                post.CommentIds.Remove(comment);
            }
            
            var result2 = PostRepository.Delete(postId);
            if (!result2)
            {
                //Save any changes made(aka removed posts deleted)
                Repository.Modify(blog);
                return new ServerError($"Unable to delete post id = {postId}. Try deleting comment before deleting blog");
            }
            blog.PostIds.Clear();
        }

        //Delete Blog
        var success = Repository.Delete(id.ToString());

        if (success)
        {
            return Ok();
        }

        //Save any changes made(aka removed comments deleted)
        Repository.Modify(blog);
        return new ServerError($"Unable to delete id = {id}.");
    }
}

