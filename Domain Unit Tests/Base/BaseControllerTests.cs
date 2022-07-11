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
    private MockController Controller { get; set; }
    public BaseControllerUnitTests()
    {
        Controller = new MockController();
    }

    [Fact]
    public void ShouldCreateNewObject()
    {
        //Arange
        var model = new MockBaseObject();
        //Act
        var result = Controller.Post(model);
        //Assert
        Assert.IsType<MockBaseObject>(result);

        var repo = Controller.GetRepository();
        Assert.True(repo.Exists(model));
    }

    public void ShouldReadAnObjectByID()
    {
        //Arange
        //Act
        //Assert
    }
    public void ShouldEditAnObject()
    {
        //Arange
        //Act
        //Assert
    }
    public void ShouldDeleteAnObject()
    {
        //Arange
        //Act
        //Assert
    }
}


public class MockController : BaseController<MockBaseObject>
{
    protected override IRepository<MockBaseObject> Repository { get; init; }

    public IRepository<MockBaseObject>  GetRepository() => Repository;


    public MockController() : base(null)
    {
        var mock = new Mock<IRepository<MockBaseObject>>();
        Repository = mock.Object;
    }
}

