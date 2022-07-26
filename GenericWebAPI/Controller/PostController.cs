using BlogAPI.Application.ApiModels;
using BlogAPI.Storage.DatabaseModels;
using Domain.ActionResults;
using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
            if (!BlogRepository.Exists(model.BlogID))
            {
                ModelState.AddModelError(nameof(model.BlogID), "Blog ID is invalid. Couldn't find the blog you are posting on!");
                return new BadRequestObjectResult(ModelState);
            }
            var post = new Post()
            {
                ID = Guid.NewGuid().ToString(),
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
            var result = base.GetById(id.ToString());
            var item = (result.Result as ObjectResult).Value as Post;

            if (item == null)
            {
                return new NotFoundObjectResult(item);
            }

            var blog = BlogRepository.GetByID(item.BlogId);
            var getblog = new GetBlog(blog);

            return new(new GetPost()
            {
                ID = item.ID,
                Title = item.Title,
                Summary = item.Summary,
                Blog = getblog,
                CommentIds = item.CommentIds,
                Content = item.Content
            });
        }

        [HttpGet("List")]
        public ActionResult<List<GetPost>> GetAll([FromQuery]Guid BlogId)
        {

            var blog = BlogRepository.GetByID(BlogId.ToString());
            if (blog == null)
            {
                return new NotFoundObjectResult(BlogId);
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
                    Title = post.Title,
                    CommentIds = post.CommentIds
                });
            }

            return new ObjectResult(GetPosts);
        }

        [HttpPost("Update")]
        public ActionResult<Post> Modify(ModifyPost model)
        {
            var blog = new Post()
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
            return base.Post(blog);
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
            foreach (var (comment, result) in
            from comment in post.CommentIds
            let result = CommentRepository.Delete(comment)
            select (comment, result))
            {
                if (!result)
                {
                    //Save any changes made(aka removed comments deleted)
                    Repository.Modify(post);
                    return new ServerError($"Unable to delete comment id = {comment}. Try deleting comment before deleting post!");
                }

                post.CommentIds.Remove(comment);
            }

            //Delete Post
            RemoveThisPostFromBlogs(post);
            
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
    }
}
