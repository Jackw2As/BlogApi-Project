using BlogAPI.Storage.DatabaseModels;
using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class GetBlog : BaseObject
    {
        public string Name { get; set; }
        public string? Summary { get; set;  }

        public GetBlog()
        {
            Name = String.Empty;
        }
        public GetBlog(Blog blog)
        {
            Name = blog.Name;
            Summary = blog.Summary;
            ID = blog.ID;
        }
    }
}
