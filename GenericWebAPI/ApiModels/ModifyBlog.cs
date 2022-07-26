using BlogAPI.Storage.DatabaseModels;

namespace BlogAPI.Application.ApiModels
{
    public class ModifyBlog : DataObject
    {
        public string Name { get; set; }
        public string? Summary { get; set; }
        public List<string> PostIds { get; set; }

        public ModifyBlog()
        {
            Name = String.Empty;
            Summary = null;
            PostIds = new();
        }
        public ModifyBlog(GetBlog getBlog)
        {
            ID = getBlog.ID;
            Name = getBlog.Name;
            Summary = getBlog.Summary;
            PostIds = getBlog.PostIds;
        }
        
    }
}
