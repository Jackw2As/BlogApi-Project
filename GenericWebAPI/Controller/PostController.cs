using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Storage.DatabaseModels;

namespace Application.Controller
{
    [Authorize()]
    public class PostController : BaseController<PostModel>
    {
        protected override IRepository<PostModel> Repository { get; init; }
        public PostController(IRepository<PostModel> repository)
        {
            Repository = repository;
        }

        [AllowAnonymous]
        public override IActionResult Get(Guid Id)
        {
            return base.Get(Id);
        }
    }
}
