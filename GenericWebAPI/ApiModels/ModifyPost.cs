using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class ModifyPost : BaseObject
    {
        public ModifyPost(GetPost getPost)
        {
            ID = getPost.ID;
            Title = getPost.Title;
            Summary = getPost.Summary;
            Content = getPost.Content;
            DateModfied = DateTime.UtcNow;
        }

        public string? Summary { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateModfied { get; }
    }
}
