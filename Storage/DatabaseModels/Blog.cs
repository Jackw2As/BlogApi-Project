namespace BlogAPI.Storage.DatabaseModels
{
    public class Blog : DataObject
    {
        List<Post> Post { get; set; }

        public string Name { get; set; }
    }
}
