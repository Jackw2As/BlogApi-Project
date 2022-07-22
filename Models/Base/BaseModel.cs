namespace Domain.Base
{
    public abstract class BaseObject
    {
        public virtual string ID { get; set; }

        public BaseObject()
        {
            ID = string.Empty;
        }
        public BaseObject(Guid id)
        {
            ID = id.ToString();
        }
        public BaseObject(string id)
        {
            ID = id;
        }
    }
}
