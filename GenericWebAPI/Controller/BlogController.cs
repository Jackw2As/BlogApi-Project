using BlogAPI.Storage.DatabaseModels;
using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controller
{
    [Authorize()]
    public class BlogController : BaseController<Blog>
    {
        protected override IRepository<Blog> Repository { get; init; }
        public BlogController(IRepository<Blog> repository)
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
