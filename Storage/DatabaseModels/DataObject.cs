using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Storage.DatabaseModels
{
    public class DataObject : BaseObject
    {
        [Key]
        public override Guid ID { get => base.ID; init => base.ID = value; }

        public override bool Equals(object? obj)
        {
            if(obj is DataObject)
            {
                var other = (DataObject)obj;
                return other.ID == ID;
            }

            return base.Equals(obj);
        }

    }
}