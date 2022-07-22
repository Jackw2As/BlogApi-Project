using Domain.Base;
using Domain.Interface;

namespace BlogAPI.Domain.Base;

public class MockRepository<T> : IRepository<T> where T : BaseObject
{
    List<T> Storage { get; }

    public MockRepository(List<T>? SeedData = null)
    {
        Storage = new List<T>();
        if (SeedData != null)
        {
            Storage.AddRange(SeedData);
        }
    }

    public bool Save(T model)
    {
        Storage.Add(model);
        return true;
    }

    public T GetByID(string Id) => Storage.First(p => p.ID == Id);

    public bool Modify(T model)
    {
        Storage.Remove(model);
        Storage.Add(model);
        return true;
    }

    public bool Delete(string Id)
    {
        var model = Storage.Find(p => p.ID == Id);
        if(model == null) return false;
        Storage.Remove(model);
        return true;
    }

    public IEnumerable<T> GetByQuery(Func<T, bool> query)
    {
        return Storage.Where(query);
    }

    public bool Exists(string Id)
    {
        if(Storage.Where(p=> p.ID == Id).Count() > 0) return true;
        return false;
    }

    public bool Exists(T Model)
    {
        if (Storage.Where(p => p == Model).Count() > 0) return true;
        return false;
    }

    public bool Exists(Func<T, bool> query)
    {
        if (Storage.Where(query).Count() > 0) return true;
        return false;
    }
}