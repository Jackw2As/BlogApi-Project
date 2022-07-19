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
    public IRepository<Post> PostRepository { get; }

    public CommentController(IRepository<Comment> repository, IRepository<Post> postRepository) : base(repository)
    {
        PostRepository = postRepository;
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
            PostId = model.Post.ID,
            Username = model.Username,
        };

        return base.Post(comment);
    }

    [HttpGet]
    public ActionResult<GetComment> GetById(Guid id)
    {
        var item = base.GetById(id).Value;
        if(item == null)
        {
            return NotFound(item);
        }

        var post = PostRepository.GetByID(item.PostId);

        return new(new GetComment()
        {
            ID = item.ID,
            Content = item.Content,
            Username = item.Username,
            Post = post
        });
    }
}
