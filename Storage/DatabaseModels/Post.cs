
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Storage.DatabaseModels
{
    public class Post : DataObject
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        [MaxLength(255)]
        public string? Summary { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        [Required]
        public string BlogId { get; set; }

        public List<string> CommentIds { get; set; } = new();

        public Post()
        {
            CommentIds = new List<string>();
            Title = String.Empty;
            Content = String.Empty;
            BlogId = String.Empty;
            if(Summary != null)
            {
                int substringLength = 250;
                if(Summary.Length < 250)
                {
                    substringLength = Summary.Length;
                }
                Summary = Content.Substring(0, substringLength);
            }
        }
    }
}
