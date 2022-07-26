using BlogAPI.Storage.DatabaseModels;
using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class CreatePost
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Summary { get; set; }
        public string BlogID { get; set; }

        public CreatePost()
        {
            Title = String.Empty;
            Content = String.Empty;
            BlogID = String.Empty;
        }
        public CreatePost(string title, string content, string blogID, string? summary = null)
        {
            Title = title;
            Content = content;
            Summary = summary;
            BlogID = blogID;
        }
        public CreatePost(string title, string content, GetBlog blog, string? summary = null)
        {
            Title = title;
            Content = content;
            Summary = summary;
            BlogID = blog.ID;
        }
    }
}