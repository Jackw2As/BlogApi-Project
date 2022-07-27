using BlogAPI.Storage.DatabaseModels;
using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class GetComment : BaseObject
    {
        public string Username { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public GetComment()
        {
            Username = String.Empty;
            Content = String.Empty;
            DateCreated = DateTime.MinValue;
        }
        public GetComment(Comment comment)
        {
            ID = comment.ID;
            Username = comment.Username;
            Content = comment.Content;
            DateCreated = comment.DateCreated;
        }
    }
}
