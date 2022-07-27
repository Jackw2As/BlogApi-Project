using BlogAPI.Application.ApiModels;
using BlogAPI.Storage.DatabaseModels;
using Domain.ActionResults;
using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;

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
        if (!PostRepository.Exists(model.PostId))
        {
            ModelState.AddModelError(nameof(model.PostId), "Post ID is invalid. Couldn't find the Post you are commenting on!");
            return new BadRequestObjectResult(ModelState);
        }
        Comment comment = createComment(model);

        return base.Post(comment);
    }

    [HttpGet]
    public ActionResult<GetComment> GetById([FromQuery]Guid id)
    {
        var result = base.GetById(id.ToString());
        var comment = (result.Result as ObjectResult).Value as Comment;
        if (comment == null)
        {
            return new NotFoundObjectResult(comment);
        }

        var post = PostRepository.GetByID(comment.PostId);
        if (post == null)
        {
            return new NotFoundObjectResult(post);
        }
        return new(new GetComment(comment));
    }

    [HttpGet("List")]
    public ActionResult<List<GetComment>> GetAll([FromQuery] Guid PostId)
    {
        try
        {
            var post = PostRepository.GetByID(PostId.ToString());
        }
        catch (Exception)
        {
            return new NotFoundObjectResult(PostId);
        }

        var comments = Repository.GetByQuery(comment => comment.PostId == PostId.ToString());
        var GetComments = new List<GetComment>();
        foreach (var comment in comments)
        {
            GetComments.Add(new GetComment(comment));
        }

        return new ObjectResult(GetComments);
    }

    [HttpPost("Update")]
    public ActionResult<Comment> Modify(ModifyComment model)
    {
        Comment comment = modifyComment(model);
        return base.Post(comment);
    }

    [HttpDelete]
    public ActionResult Delete([FromQuery] Guid id)
    {
        if (!Repository.Exists(id.ToString()))
        {
            ModelState.AddModelError(nameof(id), "id is invalid");
            return new BadRequestObjectResult(ModelState);
        }

        RemovePostFromComment(id);

        var success = Repository.Delete(id.ToString());

        if (success)
        {
            return Ok();
        }

        //Save any changes made(aka removed comments deleted)
        return new ServerError($"Unable to delete id = {id}.");
    }

    private void RemovePostFromComment(Guid id)
    {
        var posts = PostRepository.GetByQuery(post => post.CommentIds.Contains(id.ToString()));
        if (posts.Any())
        {
            foreach (var post in posts)
            {
                post.CommentIds.Remove(id.ToString());
                PostRepository.Modify(post);
            }
        }
    }

    #region Helper
    private static Comment createComment(CreateComment model)
    {
        return new Comment()
        {
            ID = Guid.NewGuid().ToString(),
            Content = model.Content,
            DateCreated = DateTime.UtcNow,
            DateModfied = DateTime.UtcNow,
            PostId = model.PostId.ToString(),
            Username = model.Username,
        };
    }
    private Comment modifyComment(ModifyComment model)
    {
        var comment = Repository.GetByID(model.ID);

        comment.Content = model.Content;
        comment.DateCreated = DateTime.UtcNow;
        return comment;
    }
    #endregion
}
