using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting.Controllers.Actor
{
    public class ActorController_DeleteTests
    {
        private readonly ActorController _actorController;
        private readonly Mock<IActorService> _mockActorService;
        int actorId = 1;

        public ActorController_DeleteTests()
        {
            _mockActorService = new Mock<IActorService>();
            _actorController = new ActorController(_mockActorService.Object);
        }
        [Fact]
        public async Task Delete_ReturnsOkResult_WhenDeleteIsSuccessful()
        {
            _mockActorService.Setup(service => service.Delete(actorId))
                .ReturnsAsync(true);
            var result = await _actorController.Delete(actorId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(response.status);
        }
        [Fact]
        public async Task Delete_ReturnsNotFound_WhenItemDoesNotExis()
        {
            _mockActorService.Setup(service => service.Delete(actorId))
                .ReturnsAsync(false);
            var result = await _actorController.Delete(actorId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(notFoundResult.Value);
            Assert.False(response.status);
            Assert.Equal("Actor couldn't be deleted", response.msg);
        }
        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenIdIsInvalid()
        {
            int actorId = -1;
            var result = await _actorController.Delete(actorId);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(notFoundResult.Value);
            Assert.False(response.status);
            Assert.Equal("Actor couldn't be deleted", response.msg);
        }
        [Fact]
        public async Task Delete_ReturnsServerError_WhenExceptionThrown()
        {
            _mockActorService.Setup(service => service.Delete(actorId))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));
            var result = await _actorController.Delete(actorId);

            var serverErrorREsult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorREsult.StatusCode);
            var response = Assert.IsType<Response<bool>>(serverErrorREsult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }
    }
}
