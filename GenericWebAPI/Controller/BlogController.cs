using BlogAPI.Application.ApiModels;
using BlogAPI.Storage.DatabaseModels;
using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Application.Controller;

public class BlogController : BaseController<Blog>
{
    public IRepository<Post> PostRepository { get; }

    public BlogController(IRepository<Blog> repository, IRepository<Post> PostRepository) : base(repository)
    {
        this.PostRepository = PostRepository;
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
}

