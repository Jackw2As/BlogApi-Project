using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class ModifyPost : BaseObject
    {
        public string? Summary { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public ModifyPost()
        {
            Title = String.Empty;
            Content = String.Empty;
        }
        public ModifyPost(GetPost getPost)
        {
            ID = getPost.ID;
            Title = getPost.Title;
            Summary = getPost.Summary;
            Content = getPost.Content;
        }
    }
}
