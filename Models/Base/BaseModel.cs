namespace Domain.Base
{
    public abstract class BaseObject
    {
        public virtual Guid ID { get; init; }

        public BaseObject()
        {
            ID = Guid.NewGuid();
        }
    }
}
