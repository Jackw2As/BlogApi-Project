using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Storage.DatabaseModels
{
    public class DataObject : BaseObject
    {
        [Key]
        public override Guid ID { get => base.ID; init => base.ID = value; }

    }
}