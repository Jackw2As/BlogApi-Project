
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Storage.DatabaseModels
{
    public class Post : DataObject
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string? Summary { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

    }
}
