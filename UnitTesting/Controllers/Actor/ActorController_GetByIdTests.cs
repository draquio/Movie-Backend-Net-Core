using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.DTO.Actor;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;


namespace UnitTesting.Controllers.Actor
{
    public class ActorController_GetByIdTests
    {
        private readonly ActorController _actorController;
        private readonly Mock<IActorService> _mockActorService;
        int actorId = 1;

        public ActorController_GetByIdTests()
        {
            _mockActorService = new Mock<IActorService>();
            _actorController = new ActorController(_mockActorService.Object);
        }
        [Fact]
        public async Task GetById_ReturnsOkResult_WithItem()
        {
            ActorDTO actor = new ActorDTO { Id = 1, Name = "Actor 1" };
            _mockActorService.Setup(service => service.GetById(actorId))
                .ReturnsAsync(actor);
            var result = await _actorController.GetActor(actorId);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<ActorDTO>>(okResult.Value);
            Assert.Equal(actorId, response.value.Id);
        }
        [Fact]
        public async Task GetById_ReturnsNotFound_WhenItemDoesNotExist()
        {
            _mockActorService.Setup(service => service.GetById(actorId))
                .ReturnsAsync((ActorDTO)null);
            var result = await _actorController.GetActor(actorId);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = Assert.IsType<Response<ActorDTO>>(notFoundResult.Value);
            Assert.False(response.status);
            Assert.Equal($"Actor with ID {actorId} not found", response.msg);
        }
        [Fact]
        public async Task GetById_ThrowsException_WhenIdIsInvalid()
        {
            int actorId = -1;
            _mockActorService.Setup(service => service.GetById(actorId))
                .ThrowsAsync(new ArgumentException("Invalid ID"));
            var result = await _actorController.GetActor(actorId);

            var badRequestResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, badRequestResult.StatusCode);
            var response = Assert.IsType<Response<ActorDTO>>(badRequestResult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Invalid ID", response.msg);
        }
        [Fact]
        public async Task GetById_ReturnsServerError_WhenExceptionThrown()
        {
            _mockActorService.Setup(service => service.GetById(actorId))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));
            var result = await _actorController.GetActor(actorId);

            var serverErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorResult.StatusCode);
            var response = Assert.IsType<Response<ActorDTO>>(serverErrorResult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }
    }
}
