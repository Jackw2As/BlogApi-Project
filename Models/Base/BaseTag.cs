using System.ComponentModel.DataAnnotations;

namespace Domain.Base
{
    public class BaseTag
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}