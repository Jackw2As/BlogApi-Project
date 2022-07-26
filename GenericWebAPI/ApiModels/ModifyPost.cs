using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class ModifyPost : BaseObject
    {
        public string? Summary { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string BlogID { get; set; }
        public List<string> CommentIds { get; set; }

        public ModifyPost()
        {
            Title = String.Empty;
            Content = String.Empty;
            BlogID = String.Empty;
            CommentIds = new();
        }
        public ModifyPost(GetPost getPost)
        {
            ID = getPost.ID;
            Title = getPost.Title;
            Summary = getPost.Summary;
            Content = getPost.Content;
            DateCreated = DateTime.UtcNow;
            BlogID = getPost.Blog.ID;
            CommentIds = getPost.CommentIds;
        }
    }
}
