
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.DTO.Movie;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting.Controllers.Movie
{
    public class MovieController_CreateTests
    {
        private readonly MovieController _movieController;
        private readonly Mock<IMovieService> _mockMovieService;
        CreateMovieDTO newMovieDTO = new CreateMovieDTO { Title = "Movie 1" };
        MovieDTO createdMovie = new MovieDTO { Id = 1, Title = "Movie 1" };

        public MovieController_CreateTests()
        {
            _mockMovieService = new Mock<IMovieService>();
            _movieController = new MovieController(_mockMovieService.Object);
        }
        [Fact]
        public async Task Create_ReturnsOkResult_WithCreatedItem()
        {
            _mockMovieService.Setup(service => service.Create(newMovieDTO))
                .ReturnsAsync(createdMovie);
            var result = await _movieController.Create(newMovieDTO);

            var okResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var response = Assert.IsType<Response<MovieDTO>>(okResult.Value);
            Assert.Equal(createdMovie.Id, response.value.Id);
        }
        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelIsInvalid()
        {
            _movieController.ModelState.AddModelError("Title", "Required");
            var result = await _movieController.Create(newMovieDTO);

            var badRequetResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var response = Assert.IsType<Response<MovieDTO>>(badRequetResult.Value);
            Assert.False(response.status);
            Assert.Contains("Required", response.errors.First());
        }
        [Fact]
        public async Task Create_ReturnsServerError_WhenExceptionThrown()
        {
            _mockMovieService.Setup(service => service.Create(newMovieDTO))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));
            var result = await _movieController.Create(newMovieDTO);

            var serverErrorresult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorresult.StatusCode);
            var response = Assert.IsType<Response<MovieDTO>>(serverErrorresult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }
    }
}
