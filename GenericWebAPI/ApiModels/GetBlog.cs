using BlogAPI.Storage.DatabaseModels;
using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class GetBlog : BaseObject
    {
        public string Name { get; set; }
        public string? Summary { get; set; }

        public List<Post> Posts { get; set; }
    }
}
