using BlogAPI.Storage.DatabaseModels;
using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class GetComment : BaseObject
    {
        public string Username { get; set; }
        public string Content { get; set; }
        public GetPost Post { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public GetComment()
        {
            Username = String.Empty;
            Content = String.Empty;
            Post = new GetPost();
            DateCreated = DateTime.Now;
        }
        public GetComment(Comment comment, GetPost post)
        {
            ID = comment.ID;
            Username = comment.Username;
            Content = comment.Content;
            DateCreated = comment.DateCreated;

            if (comment.PostId != post.ID.ToString())
            {
                throw new ArgumentException("Post is not related to Comment! Post has to be comment Parent.");
            }
            Post = post;
        }
    }
}
