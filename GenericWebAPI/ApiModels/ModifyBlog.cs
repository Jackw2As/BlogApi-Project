using BlogAPI.Storage.DatabaseModels;

namespace BlogAPI.Application.ApiModels
{
    public class ModifyBlog : DataObject
    {
        public string Name { get; set; }
        public string? Summary { get; set; }

        public ModifyBlog()
        {
            Name = String.Empty;
            Summary = null;
        }
        public ModifyBlog(GetBlog getBlog)
        {
            ID = getBlog.ID;
            Name = getBlog.Name;
            Summary = getBlog.Summary;
        }
        public ModifyBlog(string name, string? summary = null)
        {
            Name = name;
            Summary = summary;
        }
    }
}
