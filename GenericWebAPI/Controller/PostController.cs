using Domain.Base;
using Domain.Interface;
using Storage.DatabaseModels;

namespace Application.Controller
{
    public class PostController : BaseController<PostModel>
    {
        protected override IRepository<PostModel> Repository { get; init; }
        public PostController(IRepository<PostModel> repository)
        {
            Repository = repository;
        }
    }
}
