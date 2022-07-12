using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Storage.DatabaseModels
{
    public class Blog : DataObject
    {
        [Required]
        [StringLength(24, MinimumLength = 4)]
        public string Name { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 1)]
        public string Summary { get; set; }

        public List<Post> Posts { get; set; } = new();
    }
}
