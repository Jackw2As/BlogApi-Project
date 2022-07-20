using BlogAPI.Storage.DatabaseModels;
using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class CreateComment : BaseObject
    {
        public string Username { get; set; }
        public string Content { get; set; }
        public Guid PostId { get; set; }

        public CreateComment(string username, string content, Guid postID)
        {
            Username = username;
            Content = content;
            PostId = postID;
        }
        public CreateComment(string username, string content, GetPost post)
        {
            Username = username;
            Content = content;
            PostId = post.ID;
        }
    }
}
