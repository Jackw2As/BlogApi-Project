using BlogAPI.Storage.DatabaseModels;
using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class CreateComment
    {
        public string Username { get; set; }
        public string Content { get; set; }
        public string PostId { get; set; }

        public CreateComment(string username, string content, string postID)
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

        public CreateComment()
        {
            
        }
    }
}
