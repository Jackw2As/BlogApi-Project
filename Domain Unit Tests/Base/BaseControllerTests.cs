using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Domain.Base;

public class BaseControllerUnitTests
{

    #region Create Tests
    [Fact]
    public void ShouldCreateNewObject()
    {
        //Arange
        var Controller = new MockController();
        var model = new MockBaseObject();

        //Act
        var result = Controller.Post(model);
        //Assert
        Assert.IsType<ActionResult<MockBaseObject>>(result);
        Assert.Equal(model, ( (ObjectResult) result.Result ).Value);

        var repo = Controller.GetRepository();
        Assert.True(repo.Exists(model));
    }

    [Fact]
    public void ShouldNOTCreateNewObjectValidError()
    {
        //Arange
        var Controller = new MockController();
        var model = new MockBaseObject();

        //Act
        Controller.ModelState.AddModelError("", "Test");
        var result = Controller.Post(model);

        //Assert
        var objectResult= result.Result as ObjectResult;
        
        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.True(badRequest.StatusCode == 400);
        Assert.IsType<SerializableError>(badRequest.Value);

        var repo = Controller.GetRepository();
        Assert.False(repo.Exists(model));
    }

    #endregion

    #region Read Tests

    #endregion
    [Fact]
    public void ShouldReadAnObjectByID()
    {
        //Arange
        var Controller = new MockController();
        //Act
        //Assert
    }

    [Fact]
    public void ShouldNotReadAnObjectByID()
    {
        //Arange
        var Controller = new MockController();
        //Act
        //Assert
    }

    #region Edit Tests

    #endregion

    #region Delete Tests
    #endregion

    public void ShouldEditAnObject()
    {
        //Arange
        var Controller = new MockController();
        //Act
        //Assert
    }
    public void ShouldDeleteAnObject()
    {
        //Arange
        var Controller = new MockController();
        //Act
        //Assert
    }
}


public class MockController : BaseController<MockBaseObject>
{
    public IRepository<MockBaseObject>  GetRepository() => Repository;
    public MockController(List<MockBaseObject>? SeedData = null) : base(new MockRepository<MockBaseObject>(SeedData))
    {
    }
}

public class MockRepository<T> : IRepository<T> where T : BaseObject
{
    List<T> Storage { get; }

    public MockRepository(List<T>? SeedData = null)
    {
        if (SeedData == null)
            Storage = new List<T>();
        else
            Storage = SeedData;
    }

    public bool Save(T model)
    {
        Storage.Add(model);
        return true;
    }

    public T GetByID(Guid Id) => Storage.First(p => p.ID == Id);

    public bool Modify(T model)
    {
        Storage.Remove(model);
        Storage.Add(model);
        return true;
    }

    public bool Delete(Guid Id)
    {
        var model = Storage.Find(p => p.ID == Id);
        if(model != null) return false;
        Storage.Remove(model);
        return true;
    }

    public IEnumerable<T> GetByQuery(Func<T, bool> query)
    {
        return Storage.Where(query);
    }

    public bool Exists(Guid Id)
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