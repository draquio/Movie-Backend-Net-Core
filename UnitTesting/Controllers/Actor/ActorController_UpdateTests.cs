using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.DTO.Actor;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting.Controllers.Actor
{
    public class ActorController_UpdateTests
    {
        private readonly ActorController _actorController;
        private readonly Mock<IActorService> _mockActorService;
        ActorDTO actor = new ActorDTO { Id = 1, Name = "Actor 1" };
        int actorId = 1;

        public ActorController_UpdateTests()
        {
            _mockActorService = new Mock<IActorService>();
            _actorController = new ActorController(_mockActorService.Object);
        }
        [Fact]
        public async Task Update_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            _mockActorService.Setup(service => service.Update(actor))
            .ReturnsAsync(true);
            var result = await _actorController.Update(actor, actorId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(response.value);
        }
        [Fact]
        public async Task Update_ReturnsNotFound_WhenItemDoesNotExist()
        {
            _mockActorService.Setup(service => service.Update(actor))
            .ReturnsAsync(false);
            var result = await _actorController.Update(actor, actorId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(notFoundResult.Value);
            Assert.False(response.value);
            Assert.Equal("Actor couldn't be updated", response.msg);
        }
        [Fact]
        public async Task Update_ReturnsBadRequest_WhenModelIsInvalid()
        {
            _actorController.ModelState.AddModelError("Name", "Required");
            var result = await _actorController.Update(actor, actorId);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(badRequestResult.Value);
            Assert.False(response.status);
            Assert.Contains("Required", response.errors.First());
        }
        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdIsInvalid()
        {
            int actorId = 2;
            var result = await _actorController.Update(actor, actorId);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(badRequestResult.Value);
            Assert.False(response.status);
            Assert.Equal("Actor can't be null or ID mismatch", response.msg);
        }
        [Fact]
        public async Task Update_ReturnsServerError_WhenExceptionThrown()
        {
            _mockActorService.Setup(service => service.Update(actor))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));
            var result = await _actorController.Update(actor, actorId);

            var serverErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorResult.StatusCode);
            var response = Assert.IsType<Response<bool>>(serverErrorResult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }
    }
}
