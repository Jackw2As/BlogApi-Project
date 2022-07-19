using BlogAPI.Storage.DatabaseModels;
using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class CreatePost : BaseObject
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Summary { get; set; }

        public Blog Blog { get; set; }

        public CreatePost(string title, string content, Blog blog, string? summary = null)
        {
            Title = title;
            Content = content;
            Blog = blog;
            Summary = summary;
        }
    }
}