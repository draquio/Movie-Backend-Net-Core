
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting.Controllers.Movie
{
    public class MovieController_DeleteTests
    {
        private readonly MovieController _movieController;
        private readonly Mock<IMovieService> _mockMovieService;
        int movieId = 1;

        public MovieController_DeleteTests()
        {
            _mockMovieService = new Mock<IMovieService>();
            _movieController = new MovieController(_mockMovieService.Object);
        }
        [Fact]
        public async Task Delete_ReturnsOkResult_WhenDeleteIsSuccessful()
        {
            _mockMovieService.Setup(service => service.Delete(movieId))
                .ReturnsAsync(true);
            var result = await _movieController.Delete(movieId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(response.status);
        }
        [Fact]
        public async Task Delete_ReturnsNotFound_WhenItemDoesNotExis()
        {
            _mockMovieService.Setup(service => service.Delete(movieId))
                .ReturnsAsync(false);
            var result = await _movieController.Delete(movieId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(notFoundResult.Value);
            Assert.False(response.status);
            Assert.Equal("Movie couldn't be deleted", response.msg);
        }
        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenIdIsInvalid()
        {
            int movieId = -1;
            var result = await _movieController.Delete(movieId);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(notFoundResult.Value);
            Assert.False(response.status);
            Assert.Equal("Movie couldn't be deleted", response.msg);
        }
        [Fact]
        public async Task Delete_ReturnsServerError_WhenExceptionThrown()
        {
            _mockMovieService.Setup(service => service.Delete(movieId))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));
            var result = await _movieController.Delete(movieId);

            var serverErrorREsult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorREsult.StatusCode);
            var response = Assert.IsType<Response<bool>>(serverErrorREsult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }
    }
}
