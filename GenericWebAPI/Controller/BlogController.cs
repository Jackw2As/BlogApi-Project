using System.Net.Mime;
using BlogAPI.Application.ApiModels;
using BlogAPI.Storage.DatabaseModels;
using Domain.ActionResults;
using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace BlogAPI.Application.Controller;
public class BlogController : BaseController<Blog>
{
    //TODO: Refactor Each Endpoint into a separate class
    //TODO: Replace List method with an endpoint such as Blogs/Posts?BlogID="".
    public IRepository<Post> PostRepository { get; }
    public IRepository<Comment> CommentRepository { get; }
    public BlogController(IRepository<Blog> repository, IRepository<Post> postRepository, IRepository<Comment> commentRepository) : base(repository)
    {
        PostRepository = postRepository;
        CommentRepository = commentRepository;
    }

    [HttpPost]
    public ObjectResult Post([FromBody] CreateBlog model)
    {
        Blog blog = createBlog(model);
        if(!TryValidateModel(blog))
        {
            return BadRequest(ModelState);
        }

        return base.Post(blog);
    }

    [HttpGet]
    public ObjectResult GetById([FromQuery] string id)
    {
        var result = base.GetById(id);
        var blog = result.Value as Blog;

        if (blog == null)
        {
            return new NotFoundObjectResult(blog);
        }

        return new(new GetBlog(blog));
    }

    [HttpGet("List")]
    public ObjectResult GetAll()
    {
        var blogs = Repository.GetByQuery(blog => true);

        List<GetBlog> getBlogs = new();
        foreach (var blog in blogs)
        {
            getBlogs.Add(new(blog));
        }

        return new(getBlogs);
    }

    [HttpPost("Update")]
    public ObjectResult Modify(ModifyBlog model)
    {
        var blog = modifyBlog(model);
        return !TryValidateModel(blog) ? BadRequest(ModelState) : base.Post(blog);
    }

    [HttpDelete]
    public StatusCodeResult Delete([FromQuery] Guid id)
    {
        if (!Repository.Exists(id.ToString()))
        {
            return NotFound();
        }

        var blog = Repository.GetByID(id.ToString());

        //Delete Posts
        var postController = new PostController(PostRepository, CommentRepository, Repository);
        var posts = postController.GetAll(Guid.Parse(blog.ID)).Value as List<GetPost>;
        foreach (var post in posts)
        {
            var results = postController.Delete(Guid.Parse(post.ID));
            if (results.StatusCode != 200)
            {
                return results;
            }
        }

        //Delete Blog
        var success = Repository.Delete(id.ToString());
        if (success) return Ok();

        //Save any changes made(aka removed comments deleted)
        Repository.Modify(blog);
        return new NotFoundResult();
    }

    #region helpers
    private static Blog createBlog(CreateBlog model)
    {
        return new()
        {
            ID = Guid.NewGuid().ToString(),
            Name = model.Name,
            Summary = model.Summary ?? "My Fantastic New Blog!!!"
        };
    }

    private Blog modifyBlog(ModifyBlog model)
    {
        var blog = Repository.GetByID(model.ID);

        blog.Summary = model.Summary ?? "My Fantastic New Blog!!!";
        blog.Name = model.Name;

        return blog;
    }
    #endregion
}

