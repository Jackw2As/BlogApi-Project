using BlogAPI.Application.ApiModels;
using BlogAPI.Storage.DatabaseModels;
using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controller
{
    public class PostController : BaseController<Post>
    {
        public PostController(IRepository<Post> repository) : base(repository)
        {

        }

        [HttpPost]
        public ActionResult<Post> Post([FromBody] CreatePost model)
        {
            var post = new Post()
            {
                ID = model.ID,
                Blog = model.Blog,
                Comments = new(),
                Content = model.Content,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                Name = model.Title,
                Summary = model.Summary,
            };

            return base.Post(post);
        }

        [HttpGet]
        public ActionResult<Post> GetById(Guid id)
        {
            return base.GetById(id);
        }
    }
}
