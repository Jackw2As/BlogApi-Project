using BlogAPI.Storage.DatabaseModels;
using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controller
{
    [Authorize()]
    public class PostController : BaseController<Post>
    {
        public PostController(IRepository<Post> repository) : base(repository)
        {

        }
    }
}
