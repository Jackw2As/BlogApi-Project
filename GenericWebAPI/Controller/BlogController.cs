using BlogAPI.Application.ApiModels;
using BlogAPI.Storage.DatabaseModels;
using Domain.ActionResults;
using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
            PostIds = new()
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

        return new ObjectResult(new GetBlog(item));
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

    [HttpPost("Update")]
    public ActionResult<Blog> Modify(ModifyBlog model)
    {
        var blog = new Blog()
        {
            ID = model.ID,
            Name = model.Name,
            Summary = model.Summary ?? "My Fantastic New Blog!!!",
            PostIds = model.PostIds
        };
        return base.Post(blog);
    }

    [HttpDelete]
    public ActionResult Delete([FromQuery] Guid id)
    {
        if (!Repository.Exists(id.ToString()))
        {
            ModelState.AddModelError(nameof(id), "id is invalid");
            return new BadRequestObjectResult(ModelState);
        }

        var blog = Repository.GetByID(id.ToString());
        
        //Delete Posts
        foreach (var (postId, post) in
        from postId in blog.PostIds
        let post = PostRepository.GetByID(postId.ToString())
        select (postId, post))
        {
            //Delete Comments
            foreach (var (comment, result1) in
            from comment in PostRepository.GetByID(postId).CommentIds
            let result1 = CommentRepository.Delete(comment)
            select (comment, result1))
            {
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
        if (success) return Ok();

        //Save any changes made(aka removed comments deleted)
        Repository.Modify(blog);
        return new ServerError($"Unable to delete id = {id}.");
    }
}

