using Domain.Base;
using Domain.Interface;

namespace BlogAPI.Domain.Base;

public class MockController : BaseController<MockBaseObject>
{
    public IRepository<MockBaseObject>  GetRepository() => Repository;
    public MockController(List<MockBaseObject>? SeedData = null) : base(new MockRepository<MockBaseObject>(SeedData))
    {
    }
}
