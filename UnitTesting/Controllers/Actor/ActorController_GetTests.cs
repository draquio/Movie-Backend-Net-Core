using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.DTO.Actor;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting.Controllers.Actor
{
    public class ActorController_GetTests
    {
        private readonly ActorController _actorController;
        private readonly Mock<IActorService> _mockActorService;
        int page = 1, pageSize = 10;

        public ActorController_GetTests()
        {
            _mockActorService = new Mock<IActorService>();
            _actorController = new ActorController(_mockActorService.Object);
        }
        [Fact]
        public async Task GetList_ReturnsOkResult_WithListofItems()
        {
            _mockActorService.Setup(service => service.GetActors(page, pageSize))
                .ReturnsAsync(new List<ActorDTO>
                {
                        new ActorDTO { Id = 1, Name = "Actor 1" },
                        new ActorDTO { Id = 2, Name = "Actor 2" },
                        new ActorDTO { Id = 2, Name = "Actor 3" },
                });
            var result = await _actorController.GetList(page, pageSize);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<List<ActorDTO>>>(okResult.Value);
            var actors = response.value;
            Assert.Equal(3, actors.Count);
        }
        [Fact]
        public async Task GetList_ReturnsOkResult_WhenListIsEmpty()
        {
            _mockActorService.Setup(service => service.GetActors(page, pageSize))
                .ReturnsAsync(new List<ActorDTO>());
            var result = await _actorController.GetList(page, pageSize);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<List<ActorDTO>>>(okResult.Value);
            Assert.Empty(response.value);
        }
        [Fact]
        public async Task GetList_ReturnsServerError_WhenExceptionThrown()
        {
            _mockActorService.Setup(service => service.GetActors(page, pageSize))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));

            var result = await _actorController.GetList(page, pageSize);
            var serverErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorResult.StatusCode);
            var response = Assert.IsType<Response<List<ActorDTO>>>(serverErrorResult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }
    }
}
