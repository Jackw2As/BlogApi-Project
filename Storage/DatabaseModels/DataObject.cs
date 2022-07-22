using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Storage.DatabaseModels
{
    public class DataObject : BaseObject
    {
        public DataObject()
        {
            ID = Guid.NewGuid().ToString();
        }

        public DataObject(Guid ID) : base(ID)
        {

        }
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