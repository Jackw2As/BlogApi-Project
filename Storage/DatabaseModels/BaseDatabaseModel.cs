using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Storage.DatabaseModels
{
    public class BaseDatabaseModel : BaseModel
    {
        [Key]
        public override Guid ID { get => base.ID ; init => base.ID = value; }
    }
}