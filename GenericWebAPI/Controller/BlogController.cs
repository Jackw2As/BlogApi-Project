using BlogAPI.Application.ApiModels;
using BlogAPI.Storage.DatabaseModels;
using Domain.ActionResults;
using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Application.Controller;

public class BlogController : BaseController<Blog>
{
    public IRepository<Post> PostRepository { get; }
    public IRepository<Comment> CommentRepository { get; }
    public BlogController(IRepository<Blog> repository, IRepository<Post> postRepository, IRepository<Comment> commentRepository) : base(repository)
    {
        PostRepository = postRepository;
        CommentRepository = commentRepository;
    }

    [HttpPost]
    public ActionResult<Blog> Post([FromBody] CreateBlog model)
    {
        Blog blog = createBlog(model);
        return base.Post(blog);
    }

    [HttpGet]
    public ActionResult<GetBlog> GetById([FromQuery] string id)
    {
        var result = base.GetById(id);
        var blog = (result.Result as ObjectResult).Value as Blog;

        if (blog == null)
        {
            return new NotFoundObjectResult(blog);
        }

        return new ObjectResult(new GetBlog(blog));
    }

    [HttpGet("List")]
    public ActionResult<List<GetBlog>> GetAll()
    {
        var blogs = Repository.GetByQuery(blog => true);

        List<GetBlog> getBlogs = new List<GetBlog>();
        foreach (var blog in blogs)
        {
            getBlogs.Add(new(blog));
        }
        return new ObjectResult(getBlogs);
    }

    [HttpPost("Update")]
    public ActionResult<Blog> Modify(ModifyBlog model)
    {
        Blog blog = createBlog(model);
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

        var blog = Repository.GetByID(id.ToString());

        //Delete Posts in blog
        var results = deletePosts(blog);
        if(!results)
        {
            return new ServerError($"Unable to delete post. Try deleting posts before deleting blog");
        }

        //Delete Blog
        var success = Repository.Delete(id.ToString());
        if (success) return Ok();

        //Save any changes made(aka removed comments deleted)
        Repository.Modify(blog);
        return new ServerError($"Unable to delete id = {id}.");
    }

    private bool deleteComments(string postId, Post post)
    {
        foreach (var (comment, result) in
            from comment in PostRepository.GetByID(postId).CommentIds
            let result = CommentRepository.Delete(comment)
            select (comment, result))
        {
            if (!result)
            {
                //Save any changes made(aka removed comments deleted)
                PostRepository.Modify(post);
                return false;
            }
            post.CommentIds.Remove(comment);
        }
        return true;
    }

    private bool deletePosts(Blog blog)
    {
        foreach (var (postId, post) in
        from postId in blog.PostIds
        let post = PostRepository.GetByID(postId.ToString())
        select (postId, post))
        {
            //delete commments in post
            var results = deleteComments(postId, post);
            if (!results)
            {
                return false;
            }

            var result2 = PostRepository.Delete(postId);
            if (!result2)
            {
                //Save any changes made(aka remove reference to posts deleted)
                Repository.Modify(blog);
                return false;
            }

            blog.PostIds.Clear();
        }
        return true;
    }

    #region helpers
    private static Blog createBlog(CreateBlog model)
    {
        return new Blog()
        {
            ID = Guid.NewGuid().ToString(),
            Name = model.Name,
            Summary = model.Summary ?? "My Fantastic New Blog!!!",
            PostIds = new()
        };
    }

    private static Blog createBlog(ModifyBlog model)
    {
        return new Blog()
        {
            ID = model.ID,
            Name = model.Name,
            Summary = model.Summary ?? "My Fantastic New Blog!!!",
            PostIds = model.PostIds
        };
    }
    #endregion
}

