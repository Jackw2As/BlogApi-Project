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
            ID = model.ID,
            Name = model.Name,
            Summary = model.Summary ?? "My Fantastic New Blog!!!",
        };

        return base.Post(blog);
    }

    [HttpGet]
    public ActionResult<GetBlog> GetById(Guid id)
    {
        var item = base.GetById(id).Value;
        if (item == null)
        {
            return NotFound(item);
        }

        var posts = new List<Post>();

        foreach (var postId in item.PostIds)
        {
            var post = PostRepository.GetByID(postId);
            posts.Add(post);
        }

        return new(new GetBlog(){
            ID = item.ID,
            Name = item.Name,
            Summary = item.Summary,
            Posts = posts
        });
    }
}

