using BlogAPI.Storage.DatabaseModels;
using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controller
{
    public class BlogController : BaseController<Blog>
    {
        public BlogController(IRepository<Blog> repository) : base(repository)
        {
        }
    }
}
