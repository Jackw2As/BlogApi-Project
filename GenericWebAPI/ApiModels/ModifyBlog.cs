using BlogAPI.Storage.DatabaseModels;

namespace BlogAPI.Application.ApiModels
{
    public class ModifyBlog : DataObject
    {
        public ModifyBlog(GetBlog getBlog)
        {
            ID = getBlog.ID;
            Name = getBlog.Name;
            Summary = getBlog.Summary;
        }

        public string Name { get; set; }
        public string? Summary { get; set; }
    }
}
