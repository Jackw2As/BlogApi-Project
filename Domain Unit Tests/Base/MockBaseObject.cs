using Domain.Base;

namespace BlogAPI.Domain.Base
{
    public class MockBaseObject : BaseObject
    {
        public MockBaseObject()
        {
            ID = Guid.NewGuid();
        }
    }
}