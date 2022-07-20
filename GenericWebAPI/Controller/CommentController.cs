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
    public IRepository<Blog> BlogRepository { get; }

    public CommentController(IRepository<Comment> repository, 
                            IRepository<Post> postRepository, 
                            IRepository<Blog> blogRepository)
        : base(repository)
    {
        PostRepository = postRepository;
        BlogRepository = blogRepository;
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
            PostId = model.PostId.ToString(),
            Username = model.Username,
        };

        return base.Post(comment);
    }

    [HttpGet]
    public ActionResult<GetComment> GetById([FromQuery]Guid id)
    {
        var item = base.GetById(id).Value;
        if(item == null)
        {
            return NotFound(item);
        }

        var post = PostRepository.GetByID(Guid.Parse(item.PostId));
        var blog = BlogRepository.GetByID(Guid.Parse(post.BlogId));

        var getblog = new GetBlog(blog);
        var getPost = new GetPost(post, getblog);

        return new(new GetComment()
        {
            ID = item.ID,
            Content = item.Content,
            Username = item.Username,
            Post = getPost
        });
    }

    [HttpGet("List")]
    public ActionResult<List<GetComment>> GetAll([FromQuery(Name = "Post ID")] Guid PostId)
    {
        var post = PostRepository.GetByID(PostId);
        if (post == null)
        {
            return NotFound(PostId);
        }
        var blog = BlogRepository.GetByID(Guid.Parse(post.BlogId));
        var getblog = new GetBlog(blog);
        var getPost = new GetPost(post, getblog);

        var comments = Repository.GetByQuery(comment => comment.PostId == PostId.ToString());

        
        var GetComments = new List<GetComment>();
        foreach (var comment in comments)
        {
            GetComments.Add(
            new GetComment(comment, getPost));
        }

        return new(GetComments);
    }
}
