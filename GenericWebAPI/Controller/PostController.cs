using BlogAPI.Application.ApiModels;
using BlogAPI.Storage.DatabaseModels;
using Domain.ActionResults;
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
                              )
            : base(repository)
        {
            CommentRepository = commentRepository;
            BlogRepository = blogRepository;
        }

        [HttpPost()]
        public ActionResult<Post> Post([FromBody] CreatePost model)
        {
            if (!BlogRepository.Exists(model.BlogID))
            {
                ModelState.AddModelError(nameof(model.BlogID), "Blog ID is invalid. Couldn't find the blog you are posting on!");
                return new BadRequestObjectResult(ModelState);
            }
            var post = CreatePost(model);

            return base.Post(post);
        }

        [HttpGet()]
        public ActionResult<GetPost> GetById([FromQuery] Guid id)
        {
            var result = base.GetById(id.ToString());

            var post = (result.Result as ObjectResult).Value as Post;
            if (post == null)
            {
                return new NotFoundObjectResult(post);
            }

            var blog = BlogRepository.GetByID(post.BlogId);
            var getblog = new GetBlog(blog);

            return new(new GetPost(post, getblog));
        }

        [HttpGet("List")]
        public ActionResult<List<GetPost>> GetAll([FromQuery] Guid BlogId)
        {
            var blog = BlogRepository.GetByID(BlogId.ToString());
            if (blog == null) return new NotFoundObjectResult(BlogId);

            var posts = Repository.GetByQuery(post => post.BlogId == BlogId.ToString());

            var getBlog = new GetBlog(blog);
            var getPosts = new List<GetPost>();
            foreach (var post in posts)
            {
                getPosts.Add(new GetPost(post, getBlog));
            }

            return new ObjectResult(getPosts);
        }

        [HttpPost("Update")]
        public ActionResult<Post> Modify(ModifyPost model)
        {
            Post post = CreatePost(model);
            return base.Post(post);
        }

        [HttpDelete]
        public ActionResult Delete([FromQuery] Guid id)
        {
            if (!Repository.Exists(id.ToString()))
            {
                ModelState.AddModelError(nameof(id), "id is invalid");
                return new BadRequestObjectResult(ModelState);
            }

            var post = Repository.GetByID(id.ToString());

            var results = DeleteComments(post);
            if (!results)
            {
                return new ServerError($"Unable to delete comment. Try deleting comment before deleting post!");
            }

            //Delete Post References
            RemoveThisPostFromBlogs(post);

            //Delete Post
            var success = Repository.Delete(id.ToString());
            if (success) return Ok();

            //Save any changes made(aka removed comments deleted)
            Repository.Modify(post);
            return new ServerError($"Unable to delete id = {id}.");
        }

        private void RemoveThisPostFromBlogs(Post post)
        {
            var blogs = BlogRepository.GetByQuery(blog => blog.PostIds.Contains(post.ID));
            foreach (var blog in blogs)
            {
                blog.PostIds.Remove(post.ID);
                BlogRepository.Modify(blog);
            }
        }

        private bool DeleteComments(Post post)
        {
            foreach (var (comment, result) in
           from comment in post.CommentIds
           let result = CommentRepository.Delete(comment)
           select (comment, result))
            {
                if (!result)
                {
                    //Save any changes made(aka removed comments deleted)
                    Repository.Modify(post);
                    return false;
                }
                post.CommentIds.Remove(comment);
            }
            return true;
        }

        #region helper
        private static Post CreatePost(CreatePost model)
        {
            return new()
            {
                ID = Guid.NewGuid().ToString(),
                BlogId = model.BlogID.ToString(),
                Content = model.Content,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                Title = model.Title,
                Summary = model.Summary,
            };
        }
        private static Post CreatePost(ModifyPost model)
        {
            return new Post()
            {
                ID = model.ID,
                Content = model.Content,
                DateCreated = model.DateCreated,
                DateModified = DateTime.UtcNow,
                BlogId = model.BlogID,
                CommentIds = model.CommentIds,
                Summary = model.Summary ?? "My Fantastic New Blog!!!",
                Title = model.Title
            };
        }
        #endregion
    }
}
