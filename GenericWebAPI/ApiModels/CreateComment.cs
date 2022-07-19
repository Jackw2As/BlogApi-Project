using BlogAPI.Storage.DatabaseModels;
using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class CreateComment : BaseObject
    {
        public string Username { get; set; }
        public string Content { get; set; }
        public Post Post { get; set; }

        public CreateComment(string username, string content, Post post)
        {
            Username = username;
            Content = content;
            Post = post;
        }
    }
}
