using BlogAPI.Storage.DatabaseModels;
using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class GetComment : BaseObject
    {
        public string Username { get; set; }
        public string Content { get; set; }
        public GetPost Post { get; set; }

        public GetComment()
        {
            Username = String.Empty;
            Content = String.Empty;
            Post = new GetPost();
        }

        public GetComment(Comment comment, GetPost post)
        {
            Username = comment.Username;
            Content = comment.Content;

            if (comment.PostId != post.ID.ToString())
            {
                throw new ArgumentException("Post is not related to Comment! Post has to be comment Parent.");
            }
            Post = post;
        }
    }
}
