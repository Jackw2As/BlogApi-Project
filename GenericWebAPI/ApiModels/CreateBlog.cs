using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class CreateBlog : BaseObject
    {
        public string Name { get; set; }

        public string? Summary { get; set; }

        public CreateBlog(string name, string? summary = null)
        {
            Name = name;
            Summary = summary;
        }
    }
}
