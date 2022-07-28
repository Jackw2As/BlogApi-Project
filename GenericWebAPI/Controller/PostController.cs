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
        public ObjectResult Post([FromBody] CreatePost model)
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
        public ObjectResult GetById([FromQuery] Guid id)
        {
            var result = base.GetById(id.ToString());

            var post = result.Value as Post;
            if (post == null)
            {
                return new NotFoundObjectResult(post);
            }
            return new OkObjectResult(new GetPost(post));
        }

        [HttpGet("List")]
        public ObjectResult GetAll([FromQuery] Guid BlogId)
        {
            try
            {
                var blog = BlogRepository.GetByID(BlogId.ToString());
            }
            catch (Exception)
            {
                return new NotFoundObjectResult(BlogId);
            }
            var posts = Repository.GetByQuery(post => post.BlogId == BlogId.ToString());

            var getPosts = new List<GetPost>();
            foreach (var post in posts)
            {
                getPosts.Add(new GetPost(post));
            }

            return new OkObjectResult(getPosts);
        }

        [HttpPost("Update")]
        public ObjectResult Modify(ModifyPost model)
        {
            Post post = modifyPost(model);
            return base.Post(post);
        }

        [HttpDelete]
        public StatusCodeResult Delete([FromQuery] Guid id)
        {
            if (!Repository.Exists(id.ToString()))
            {
                ModelState.AddModelError(nameof(id), "id is invalid");
                return new BadRequestResult();
            }

            var post = Repository.GetByID(id.ToString());

            var results = DeleteComments(post);
            if (!results)
            {
                return new BadRequestResult();
            }

            //Delete Post References
            RemoveThisPostFromBlogs(post);

            //Delete Post
            var success = Repository.Delete(id.ToString());
            if (success) return new OkResult();

            //Save any changes made(aka removed comments deleted)
            Repository.Modify(post);
            return new BadRequestResult();
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
        private Post modifyPost(ModifyPost model)
        {
            var post = Repository.GetByID(model.ID);

            post.Title = model.Title;
            post.Summary = model.Summary ?? "My Fantastic New Blog!!!";
            post.DateModified = DateTime.UtcNow;
            post.Content = model.Content;

            return post;
        }
        #endregion
    }
}
