using BlogAPI.Application.ApiModels;
using BlogAPI.Storage.DatabaseModels;
using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controller
{
    public class PostController : BaseController<Post>
    {
        public IRepository<Comment> CommentRepository { get; }
        public IRepository<Blog> BlogRepository { get; }

        public PostController(
                                IRepository<Post> repository,
                                IRepository<Comment> commentRepository,
                                IRepository<Blog> blogRepository
                              ) : base(repository)
        {
            CommentRepository = commentRepository;
            BlogRepository = blogRepository;
        }

        [HttpPost]
        public ActionResult<Post> Post([FromBody] CreatePost model)
        {
            var post = new Post()
            {
                ID = model.ID,
                BlogId = model.Blog.ID,
                Content = model.Content,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                Title = model.Title,
                Summary = model.Summary,
            };

            return base.Post(post);
        }

        [HttpGet]
        public ActionResult<GetPost> GetById(Guid id)
        {
            var item = base.GetById(id).Value;
            if (item == null)
            {
                return NotFound(item);
            }

            var comments = new List<Comment>();

            foreach (var postId in item.CommentIds)
            {
                var comment = CommentRepository.GetByID(postId);
                comments.Add(comment);
            }

            var blog = BlogRepository.GetByID(item.BlogId);

            return new(new GetPost()
            {
                ID = item.ID,
                Title = item.Title,
                Summary = item.Summary,
                Comments = comments,
                Blog = blog
            });
        }
    }
}
