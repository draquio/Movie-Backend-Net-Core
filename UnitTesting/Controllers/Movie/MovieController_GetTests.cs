using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.DTO.Actor;
using MovieCRUD_NCapas.DTO.Movie;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting.Controllers.Movie
{
    public class MovieController_GetTests
    {
        private readonly MovieController _movieController;
        private readonly Mock<IMovieService> _mockMovieService;
        int page = 1, pageSize = 10;

        public MovieController_GetTests()
        {
            _mockMovieService = new Mock<IMovieService>();
            _movieController = new MovieController(_mockMovieService.Object);
        }
        [Fact]
        public async Task GetList_ReturnsOkResult_WithListofItems()
        {
            _mockMovieService.Setup(service => service.GetMovies(page, pageSize))
                .ReturnsAsync(new List<MovieDTO>
                {
                        new MovieDTO { Id = 1, Title = "Movie 1"},
                        new MovieDTO { Id = 2, Title = "Movie 2" },
                        new MovieDTO { Id = 2, Title = "Movie 3" },
                });
            var result = await _movieController.GetList(page, pageSize);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<List<MovieDTO>>>(okResult.Value);
            var movies = response.value;
            Assert.Equal(3, movies.Count);
        }
        [Fact]
        public async Task GetList_ReturnsOkResult_WhenListIsEmpty()
        {
            _mockMovieService.Setup(service => service.GetMovies(page, pageSize))
                .ReturnsAsync(new List<MovieDTO>());
            var result = await _movieController.GetList(page, pageSize);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<List<MovieDTO>>>(okResult.Value);
            Assert.Empty(response.value);
        }
        [Fact]
        public async Task GetList_ReturnsServerError_WhenExceptionThrown()
        {
            _mockMovieService.Setup(service => service.GetMovies(page, pageSize))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));

            var result = await _movieController.GetList(page, pageSize);
            var serverErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorResult.StatusCode);
            var response = Assert.IsType<Response<List<MovieDTO>>>(serverErrorResult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }
    }
}
