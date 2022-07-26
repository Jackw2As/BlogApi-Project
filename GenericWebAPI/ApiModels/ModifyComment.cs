using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class ModifyComment : BaseObject
    {
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string PostId { get; set; }
        public string Username { get; set; }

        public ModifyComment()
        {
            Content = String.Empty;
            DateCreated = DateTime.UtcNow;
            PostId = String.Empty;
            Username = String.Empty;
        }
        public ModifyComment(GetComment getComment)
        {
            ID = getComment.ID;
            Content = getComment.Content;
            DateCreated = getComment.DateCreated;
            PostId = getComment.Post.ID;
            Username = getComment.Username;
        }
    }
}
