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

        [HttpPost()]
        public ActionResult<Post> Post([FromBody] CreatePost model)
        {
            var post = new Post()
            {
                ID = model.ID,
                BlogId = model.BlogID.ToString(),
                Content = model.Content,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                Title = model.Title,
                Summary = model.Summary,
            };

            return base.Post(post);
        }

        [HttpGet()]
        public ActionResult<GetPost> GetById([FromQuery] Guid id)
        {
            var item = base.GetById(id).Value;
            if (item == null)
            {
                return NotFound(item);
            }

            var blog = BlogRepository.GetByID(Guid.Parse(item.BlogId));
            var getblog = new GetBlog(blog);

            return new(new GetPost()
            {
                ID = item.ID,
                Title = item.Title,
                Summary = item.Summary,
                Blog = getblog
            });
        }

        [HttpGet("List")]
        public ActionResult<List<GetPost>> GetAll([FromQuery]Guid BlogId)
        {

            var blog = BlogRepository.GetByID(BlogId);
            if (blog == null)
            {
                return NotFound(BlogId);
            }

            var posts = Repository.GetByQuery(post => post.BlogId == BlogId.ToString());

            var GetBlog = new GetBlog(blog);
            var GetPosts = new List<GetPost>();

            foreach (var post in posts)
            {
                GetPosts.Add(
                new GetPost()
                {
                    Blog = GetBlog,
                    Content = post.Content,
                    ID = post.ID,
                    Summary = post.Summary,
                    Title = post.Title
                });
            }

            return new(GetPosts);
        }
    }
}
