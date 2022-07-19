using BlogAPI.Application.ApiModels;
using BlogAPI.Storage.DatabaseModels;
using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Application.Controller;

public class BlogController : BaseController<Blog>
{
    public BlogController(IRepository<Blog> repository) : base(repository)
    {
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
    public ActionResult<Blog> GetById(Guid id)
    {
        return base.GetById(id);
    }
}

