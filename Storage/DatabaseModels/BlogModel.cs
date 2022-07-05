using Storage.DatabaseModels;

namespace BlogAPI.Storage.DatabaseModels
{
    public class BlogModel : BaseDatabaseModel
    {
        List<PostModel> PostModels { get; set; }

        public string Name { get; set; }
    }
}
