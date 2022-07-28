using Domain.ActionResults;
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
        Assert.Equal(model, result.Value);

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
        var objectResult = Controller.Post(model);

        //Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(objectResult);
        Assert.True(badRequest.StatusCode == 400);
        Assert.IsType<SerializableError>(badRequest.Value);

        var repo = Controller.GetRepository();
        Assert.False(repo.Exists(model));
    }

    #endregion

    #region Read Tests
    [Fact]
    public void ShouldReadAnObjectByID()
    {
        //Arange
        var testData = CreateTestData();
        var controller = new MockController(testData);
        var item = testData.First();

        //Act
        var result = controller.GetById(item.ID);

        //Assert
        Assert.NotNull(result);
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.StrictEqual(item, objectResult.Value);
    }

    [Fact]
    public void ShouldNotReadAnObjectByID()
    {
        //Arange
        var testData = CreateTestData();
        var controller = new MockController(testData);
        var id = Guid.NewGuid().ToString();

        //Act
        var result = controller.GetById(id);

        //Assert
        Assert.NotNull(result);
        var objectResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.StrictEqual(id, objectResult.Value);
    }
    #endregion

    #region Edit Tests

    [Fact]
    public void ShouldEditAnObject()
    {
        //Arange
        var testData = CreateTestData();
        var controller = new MockController(testData);

        //Act
        var item = testData.First();

        item.isEdited = true;

        var result = controller.Post(item);

        //Assert
        Assert.Equal(item, result.Value);

        var repo = controller.GetRepository();
        Assert.True(repo.Exists(item));
    }

    #endregion

    #region Delete Tests

    [Fact]
    public void ShouldDeleteAnObject()
    {
        //Arange
        var testData = CreateTestData();
        var controller = new MockController(testData);

        var item = testData.First();

        //Act
        var response = controller.Delete(item.ID);

        //Assert
        Assert.IsType<OkResult>(response);

        var repository = controller.GetRepository();
        Assert.False(repository.Exists(item));

        var list = repository.GetByQuery((a) => true);

        Assert.True(list.Count() == (testData.Count() - 1));
    }

    [Fact]
    public void ShouldNotDeleteAnObject()
    {
        //Arange
        var testData = CreateTestData();
        var controller = new MockController(testData);

        var item = new MockBaseObject();

        //Act
        var response = controller.Delete(item.ID);

        //Assert
        Assert.IsType<NotFoundResult>(response);
        Assert.True(testData.Count() == testData.Count());
    }
    #endregion

    private List<MockBaseObject> CreateTestData()
    {
        return new()
        {
            new(),
            new(),
            new(),
        };
    }
}
