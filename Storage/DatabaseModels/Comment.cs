using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Storage.DatabaseModels
{
    public class Comment : DataObject
    {
        [MaxLength(20)]
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(300, MinimumLength = 6)]
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModfied { get; set; }

        [Required]
        public Guid PostId { get; set; }

        public Comment()
        {

        }
    }
}
