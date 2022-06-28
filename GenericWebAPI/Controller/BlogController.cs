using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Storage.DatabaseModels;

namespace Application.Controller
{
    [Authorize()]
    public class BlogController : BaseController<BlogModel>
    {
        protected override IRepository<BlogModel> Repository { get; init; }
        public BlogController(IRepository<BlogModel> repository)
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
