using Domain.Base;
using Domain.Interface;
using Storage.DatabaseModels;

namespace Application.Blog
{
    public class BlogController : BaseController<BlogModel>
    {
        protected override IRepository<BlogModel> Repository { get ; init; }
        public BlogController(IRepository<BlogModel> repository)
        {
            Repository = repository;
        }
    }
}
