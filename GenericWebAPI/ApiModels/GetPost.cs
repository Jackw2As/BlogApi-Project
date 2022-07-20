using BlogAPI.Storage.DatabaseModels;
using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class GetPost : BaseObject
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Summary { get; set; }
        public GetBlog Blog { get; set; }


        public GetPost()
        {
            Title = String.Empty;
            Content = String.Empty;
            Blog = new GetBlog();
        }

        public GetPost(Post post, GetBlog blog)
        {
            Title = post.Title;
            Content = post.Content;
            Summary = post.Summary;

            if (post.BlogId != blog.ID.ToString())
            {
                throw new ArgumentException("Blog is not related to Post! Blog has to be Post's Parent.");
            }
            Blog = blog;
        }
    }
}
