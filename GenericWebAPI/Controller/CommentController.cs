using BlogAPI.Application.ApiModels;
using BlogAPI.Storage.DatabaseModels;
using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Application.Controller;

public class CommentController : BaseController<Comment>
{
    public CommentController(IRepository<Comment> repository) : base(repository)
    {

    }

    [HttpPost]
    public ActionResult<Comment> Post([FromBody] CreateComment model)
    {
        var comment = new Comment()
        {
            ID = model.ID,
            Content = model.Content,
            DateCreated = DateTime.UtcNow,
            DateModfied = DateTime.UtcNow,
            Post = model.Post,
            Username = model.Username,
        };

        return base.Post(comment);
    }

    [HttpGet]
    public ActionResult<Comment> GetById(Guid id)
    {
        return base.GetById(id);
    }
}
