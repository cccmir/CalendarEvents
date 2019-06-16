using CalendarEvents.Controllers;
using CalendarEvents.Models;
using CalendarEvents.Services;
using Microsoft.AspNetCore.Mvc;
using Autofac.Extras.Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using Moq;

namespace CalendarEvents.Tests
{
    public class EventsControllerTests : BaseTest
    {
        private AutoMock _mock = null;

        [SetUp] public void Setup()
        {
            _mock = AutoMock.GetLoose();
        }

        #region Get
        [Test] public void Get_WhenCalled_ShouldReturnOk()
        {
            //Arrange
            IEnumerable<EventModel> expectedList = TestsFacade.EventsFacade.BuildEventModelList();

            ResultService<IEnumerable<EventModel>> expected = ResultService.Ok(expectedList);

            _mock.Mock<IGenericService<EventModel>>()
                .Setup(items => items.Get(It.IsAny<IEnumerable<FilterStatement<EventModel>>>(),
                                        It.IsAny<OrderByStatement<EventModel>>(),
                                        It.IsAny<string>()))
                .Returns(() => expected);

            var controller = _mock.Create<EventsController>();

            //Act
            ActionResult<IEnumerable<EventModel>> result = controller.Get(new GenericRequest<EventModel>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);

            IEnumerable<EventModel> resultList = okResult.Value as IEnumerable<EventModel>;

            Assert.IsNotNull(resultList);
            Assert.AreEqual(resultList.Count(), expectedList.Count());
            Assert.AreEqual(resultList, expectedList, "List not equal");
        }
        [Test] public void Get_WhenCalledWithNull_ShouldReturnOk()
        {
            //Arrange
            IEnumerable<EventModel> expectedList = TestsFacade.EventsFacade.BuildEventModelList();
            ResultService<IEnumerable<EventModel>> expected = ResultService.Ok(expectedList);

            _mock.Mock<IGenericService<EventModel>>()
                .Setup(items => items.Get(It.IsAny<IEnumerable<FilterStatement<EventModel>>>(),
                                        It.IsAny<OrderByStatement<EventModel>>(),
                                        It.IsAny<string>()))
                .Returns(() => expected);

            var controller = _mock.Create<EventsController>();

            //Act
            ActionResult<IEnumerable<EventModel>> result = controller.Get(new GenericRequest<EventModel>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);

            IEnumerable<EventModel> resultList = okResult.Value as IEnumerable<EventModel>;

            Assert.IsNotNull(resultList);
            Assert.AreEqual(resultList.Count(), expectedList.Count());
            Assert.AreEqual(resultList, expectedList);
        }
        [Test] public void Get_WhenServiceHasError_ShouldReturnStatusCode500()
        {
            //Arrange
            ResultService<IEnumerable<EventModel>> expectedResultService = ResultService.Fail<IEnumerable<EventModel>>(ErrorCode.Unknown);

            _mock.Mock<IGenericService<EventModel>>()
                .Setup(items => items.Get(It.IsAny<IEnumerable<FilterStatement<EventModel>>>(),
                                        It.IsAny<OrderByStatement<EventModel>>(),
                                        It.IsAny<string>()))
                .Returns(() => expectedResultService);

            var controller = _mock.Create<EventsController>();

            //Act
            ActionResult<IEnumerable<EventModel>> result = controller.Get(new GenericRequest<EventModel>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ObjectResult>(result.Result);

            ObjectResult objectResult = result.Result as ObjectResult;
            AssertStatusCode500(objectResult, expectedResultService.ErrorCode);
        }
        [Test] public void Get_WhenServiceThrowException_ShouldReturnStatusCode500()
        {
            //Arrange
            ResultService<IEnumerable<EventModel>> expectedResultService = ResultService.Fail<IEnumerable<EventModel>>(ErrorCode.Unknown);

            _mock.Mock<IGenericService<EventModel>>()
                .Setup(items => items.Get(It.IsAny<IEnumerable<FilterStatement<EventModel>>>(),
                                        It.IsAny<OrderByStatement<EventModel>>(),
                                        It.IsAny<string>()))
                .Throws(new Exception());

            var controller = _mock.Create<EventsController>();

            //Act
            ActionResult<IEnumerable<EventModel>> result = controller.Get(new GenericRequest<EventModel>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ObjectResult>(result.Result);

            ObjectResult objectResult = result.Result as ObjectResult;            
            AssertStatusCode500(objectResult, expectedResultService.ErrorCode);
        }
        #endregion

        #region GetById
        [Test] public void GetById_RequestNotValid_ShouldReturnBadRequest()
        {
            //Arrange
            var controller = _mock.Create<EventsController>();

            //Act
            ActionResult<EventModel> actionResult = controller.Get(new Guid());

            //Assert
            AssertBadRequestResult(actionResult);
        }
        [Test] public void GetById_WhenCalled_ShouldReturnOk()
        {
            //Arrange
            EventModel expectedItem = TestsFacade.EventsFacade.BuildEventModelItem();
            ResultService<EventModel> expectedResultService = ResultService.Ok(expectedItem);

            _mock.Mock<IGenericService<EventModel>>()
                .Setup(items => items.GetById(It.IsAny<Guid>()))
                .Returns(() => expectedResultService);

            var controller = _mock.Create<EventsController>();

            //Act
            ActionResult<EventModel> actionResult = controller.Get(Guid.NewGuid());

            //Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<OkObjectResult>(actionResult.Result);

            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);

            EventModel okResultItem = okResult.Value as EventModel;

            Assert.IsNotNull(okResultItem);
            Assert.AreEqual(okResultItem.GetHashCode(), expectedItem.GetHashCode());
        }
        [Test] public void GetById_WhenServiceHasError_ShouldReturnStatusCode500()
        {
            //Arrange
            ResultService<EventModel> expectedResultService = ResultService.Fail<EventModel>(ErrorCode.Unknown);

            _mock.Mock<IGenericService<EventModel>>()
                .Setup(items => items.GetById(It.IsAny<Guid>()))
                .Returns(() => expectedResultService);

            var controller = _mock.Create<EventsController>();

            //Act
            ActionResult<EventModel> actionResult = controller.Get(Guid.NewGuid());

            //Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<ObjectResult>(actionResult.Result);

            var objectResult = actionResult.Result as ObjectResult;
            AssertStatusCode500(objectResult, expectedResultService.ErrorCode);
        }
        [Test] public void GetById_WhenServiceThrowException_ShouldReturnStatusCode500()
        {
            //Arrange
            ResultService<EventModel> expectedResultService = ResultService.Fail<EventModel>(ErrorCode.Unknown);

            _mock.Mock<IGenericService<EventModel>>()
                .Setup(items => items.GetById(It.IsAny<Guid>()))
                .Throws(new Exception());

            var controller = _mock.Create<EventsController>();

            //Act
            ActionResult<IEnumerable<EventModel>> result = controller.Get(new GenericRequest<EventModel>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ObjectResult>(result.Result);

            ObjectResult objectResult = result.Result as ObjectResult;
            AssertStatusCode500(objectResult, expectedResultService.ErrorCode);
        }
        #endregion

        #region Post
        [Test] public void Post_RequestNotValid_ShouldReturnBadRequest()
        {
            //Arrange
            var controller = _mock.Create<EventsController>();

            //Act
            controller.ModelState.AddModelError("Title", "Required");
            ActionResult<EventModel> actionResult = controller.Post(new EventModel());

            //Assert
            AssertBadRequestResult(actionResult);
        }
        [Test] public void Post_WhenCalled_ShouldReturnPost()
        {
            //Arrange
            EventModel expectedItem = TestsFacade.EventsFacade.BuildEventModelItem();

            ResultService<EventModel> expectedResultService = ResultService.Ok(expectedItem);

            _mock.Mock<IGenericService<EventModel>>()
                .Setup(items => items.Insert(It.IsAny<EventModel>()))
                .Returns(() => expectedResultService);

            var controller = _mock.Create<EventsController>();

            //Act
            ActionResult<EventModel> actionResult = controller.Post(new EventModel());

            //Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<CreatedAtActionResult>(actionResult.Result);

            var createdAtActionResult = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult.Value);

            EventModel createdAtActionResultItem = createdAtActionResult.Value as EventModel;
            RouteValueDictionary createdAtActionResultRouteValues = createdAtActionResult.RouteValues;

            Assert.IsNotNull(createdAtActionResultItem);
            Assert.AreEqual(createdAtActionResult.ActionName, "Post");
            Assert.AreEqual(createdAtActionResultRouteValues["Id"], createdAtActionResultItem.Id);
        }
        [Test] public void Post_WhenServiceHasError_ShouldReturnStatusCode500()
        {
            //Arrange
            ResultService<EventModel> expectedResultService = ResultService.Fail<EventModel>(ErrorCode.Unknown);

            _mock.Mock<IGenericService<EventModel>>()
                .Setup(items => items.Insert(It.IsAny<EventModel>()))
                .Returns(() => expectedResultService);

            var controller = _mock.Create<EventsController>();

            //Act
            ActionResult<EventModel> actionResult = controller.Post(new EventModel());

            //Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<ObjectResult>(actionResult.Result);

            var objectResult = actionResult.Result as ObjectResult;
            AssertStatusCode500(objectResult, expectedResultService.ErrorCode);
        }
        [Test] public void Post_WhenServiceThrowException_ShouldReturnStatusCode500()
        {
            //Arrange
            ResultService<EventModel> expectedResultService = ResultService.Fail<EventModel>(ErrorCode.Unknown);

            _mock.Mock<IGenericService<EventModel>>()
                .Setup(items => items.Insert(It.IsAny<EventModel>()))
                .Throws(new Exception());

            var controller = _mock.Create<EventsController>();

            //Act
            ActionResult<IEnumerable<EventModel>> result = controller.Post(new EventModel());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ObjectResult>(result.Result);

            ObjectResult objectResult = result.Result as ObjectResult;
            AssertStatusCode500(objectResult, expectedResultService.ErrorCode);
        }
        #endregion

        #region Put
        [Test] public void Put_RequestStateNotValid_ShouldReturnBadRequest()
        {
            //Arrange
            var controller = _mock.Create<EventsController>();

            //Act
            controller.ModelState.AddModelError("Title", "Required");
            ActionResult<EventModel> actionResult = controller.Put(new EventModel());

            //Assert
            AssertBadRequestResult(actionResult);
        }
        [Test] public void Put_RequestIdNotValid_ShouldReturnBadRequest()
        {
            //Arrange
            var controller = _mock.Create<EventsController>();

            //Act
            controller.ModelState.AddModelError("Id", "Required");
            ActionResult<EventModel> actionResult = controller.Put(new EventModel());

            //Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<BadRequestResult>(actionResult.Result);
        }
        [Test] public void Put_WhenCalled_ShouldReturnPut()
        {
            //Arrange
            ResultService<EventModel> expectedResultService = ResultService.Ok(new EventModel());

            _mock.Mock<IGenericService<EventModel>>()
                .Setup(items => items.Update(It.IsAny<EventModel>()))
                .Returns(() => expectedResultService);

            var controller = _mock.Create<EventsController>();

            //Act
            ActionResult actionResult = controller.Put(new EventModel());

            //Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<OkResult>(actionResult);
        }
        [Test] public void Put_WhenServiceHasError_ShouldReturnStatusCode500()
        {
            //Arrange
            ResultService<EventModel> expectedResultService = ResultService.Fail<EventModel>(ErrorCode.Unknown);            

            _mock.Mock<IGenericService<EventModel>>()
                .Setup(items => items.Update(It.IsAny<EventModel>()))
                .Returns(() => expectedResultService);

            var controller = _mock.Create<EventsController>();

            //Act
            ActionResult<EventModel> actionResult = controller.Put(new EventModel());

            //Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<ObjectResult>(actionResult.Result);

            var objectResult = actionResult.Result as ObjectResult;
            AssertStatusCode500(objectResult, expectedResultService.ErrorCode);
        }
        [Test] public void Put_WhenServiceThrowException_ShouldReturnStatusCode500()
        {
            //Arrange
            ResultService<EventModel> expectedResultService = ResultService.Fail<EventModel>(ErrorCode.Unknown);

            _mock.Mock<IGenericService<EventModel>>()
                .Setup(items => items.Update(It.IsAny<EventModel>()))
                .Throws(new Exception());

            var controller = _mock.Create<EventsController>();

            //Act
            ActionResult<IEnumerable<EventModel>> result = controller.Put(new EventModel() { Id = Guid.NewGuid() });

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ObjectResult>(result.Result);

            ObjectResult objectResult = result.Result as ObjectResult;
            AssertStatusCode500(objectResult, expectedResultService.ErrorCode);
        }
        #endregion

        #region Delete
        [Test] public void Delete_RequestNotValid_ShouldReturnBadRequest()
        {
            //Arrange
            var controller = _mock.Create<EventsController>();

            //Act
            ActionResult<EventModel> actionResult = controller.Delete(new Guid());

            //Assert
            AssertBadRequestResult(actionResult);
        }
        [Test] public void Delete_WhenCalled_ShouldReturnOk()
        {
            //Arrange
            ResultService expectedResultService = ResultService.Ok();

            _mock.Mock<IGenericService<EventModel>>()
                .Setup(items => items.Delete(It.IsAny<Guid>()))
                .Returns(() => expectedResultService);

            var controller = _mock.Create<EventsController>();

            //Act
            ActionResult actionResult = controller.Delete(Guid.NewGuid());

            //Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<OkResult>(actionResult);
        }
        [Test] public void Delete_WhenServiceHasError_ShouldReturnStatusCode500()
        {
            //Arrange
            ResultService expectedResultService = ResultService.Fail(ErrorCode.Unknown);

            _mock.Mock<IGenericService<EventModel>>()
                .Setup(items => items.Delete(It.IsAny<Guid>()))
                .Returns(() => expectedResultService);

            var controller = _mock.Create<EventsController>();

            //Act
            ActionResult actionResult = controller.Delete(Guid.NewGuid());

            //Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<ObjectResult>(actionResult);

            var objectResult = actionResult as ObjectResult;
            AssertStatusCode500(objectResult, expectedResultService.ErrorCode);
        }
        [Test] public void Delete_WhenServiceThrowException_ShouldReturnStatusCode500()
        {
            //Arrange
            ResultService<EventModel> expectedResultService = ResultService.Fail<EventModel>(ErrorCode.Unknown);

            _mock.Mock<IGenericService<EventModel>>()
                .Setup(items => items.Delete(It.IsAny<Guid>()))
                .Throws(new Exception());

            var controller = _mock.Create<EventsController>();

            //Act
            ActionResult<IEnumerable<EventModel>> result = controller.Delete(Guid.NewGuid());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ObjectResult>(result.Result);

            ObjectResult objectResult = result.Result as ObjectResult;
            AssertStatusCode500(objectResult, expectedResultService.ErrorCode);
        }
        #endregion

        [TearDown] public void CleanUp()
        {
            if (_mock != null)
                _mock.Dispose();
        }

        private void AssertBadRequestResult(ActionResult<EventModel> actionResult)
        {
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<BadRequestResult>(actionResult.Result);
        }
        private void AssertStatusCode500(ObjectResult objectResult, ErrorCode errorCode)
        {
            Assert.IsNotNull(objectResult.Value);
            Assert.IsNotNull(objectResult.StatusCode);
            Assert.AreEqual(objectResult.StatusCode, 500);
            Assert.AreEqual((ErrorCode)objectResult.Value, errorCode);
        }

    }

}