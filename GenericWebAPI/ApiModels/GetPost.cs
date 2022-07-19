using BlogAPI.Storage.DatabaseModels;
using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class GetPost : BaseObject
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Summary { get; set; }
        public Blog Blog { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
