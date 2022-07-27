using BlogAPI.Storage.DatabaseModels;
using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class GetPost : BaseObject
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Summary { get; set; }

        public GetPost()
        {
            Title = String.Empty;
            Content = String.Empty;
        }

        public GetPost(Post post)
        {
            Title = post.Title;
            Content = post.Content;
            Summary = post.Summary;
            ID = post.ID;
        }
    }
}
