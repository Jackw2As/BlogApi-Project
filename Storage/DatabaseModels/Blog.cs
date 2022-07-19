using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public IEnumerable<Guid> PostIds { get; set; }

        public Blog()
        {
            Name = "Enter Name Here!";
            Summary = "Default Summary Text";
        }
    }
}
