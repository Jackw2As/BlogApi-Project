using Domain.Base;

namespace BlogAPI.Domain.Base
{
    public class MockBaseObject : BaseObject
    {
        public bool isEdited { get; set; } = false;

        public MockBaseObject()
        {
            ID = Guid.NewGuid().ToString();
        }
    }
}