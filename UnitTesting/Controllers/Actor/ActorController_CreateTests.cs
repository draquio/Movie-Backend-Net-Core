using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.DTO.Actor;
using MovieCRUD_NCapas.DTO.Category;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting.Controllers.Actor
{
    public class ActorController_CreateTests
    {
        private readonly ActorController _actorController;
        private readonly Mock<IActorService> _mockActorService;
        ActorDTO newActorDTO = new ActorDTO { Id = 1, Name = "Actor 1" };
        ActorDTO createdActor = new ActorDTO { Id = 1, Name = "Actor 1" };

        public ActorController_CreateTests()
        {
            _mockActorService = new Mock<IActorService>();
            _actorController = new ActorController(_mockActorService.Object);
        }
        [Fact]
        public async Task Create_ReturnsOkResult_WithCreatedItem()
        {
            _mockActorService.Setup(service => service.Create(newActorDTO))
                .ReturnsAsync(createdActor);
            var result = await _actorController.Create(newActorDTO);

            var okResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var response = Assert.IsType<Response<ActorDTO>>(okResult.Value);
            Assert.Equal(createdActor.Id, response.value.Id);
        }
        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelIsInvalid()
        {
            _actorController.ModelState.AddModelError("Name", "Required");
            var result = await _actorController.Create(newActorDTO);

            var badRequetResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var response = Assert.IsType<Response<ActorDTO>>(badRequetResult.Value);
            Assert.False(response.status);
            Assert.Contains("Required", response.errors.First());
        }
        [Fact]
        public async Task Create_ReturnsServerError_WhenExceptionThrown()
        {
            _mockActorService.Setup(service => service.Create(newActorDTO))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));
            var result = await _actorController.Create(newActorDTO);

            var serverErrorresult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorresult.StatusCode);
            var response = Assert.IsType<Response<ActorDTO>>(serverErrorresult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }
    }
}
