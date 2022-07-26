using BlogAPI.Storage.DatabaseModels;
using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class GetBlog : BaseObject
    {
        public string Name { get; set; }
        public string? Summary { get; set;  }
        public List<string> PostIds { get; set; }

        public GetBlog()
        {
            Name = String.Empty;
            PostIds = new List<string>();
        }

        public GetBlog(Blog blog)
        {
            Name = blog.Name;
            Summary = blog.Summary;
            ID = blog.ID;
            PostIds = blog.PostIds;
        }
    }
}
