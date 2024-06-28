using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.DTO.Movie;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;


namespace UnitTesting.Controllers.Movie
{
    public class MovieController_UpdateTests
    {
        private readonly MovieController _movieController;
        private readonly Mock<IMovieService> _mockMovieService;
        int movieId = 1;
        MovieDTO movie = new MovieDTO { Id = 1, Title = "Movie 1" };
        public MovieController_UpdateTests()
        {
            _mockMovieService = new Mock<IMovieService>();
            _movieController = new MovieController(_mockMovieService.Object);
        }
        [Fact]
        public async Task Update_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            _mockMovieService.Setup(service => service.Update(movie))
                .ReturnsAsync(true);
            var result = await _movieController.Update(movie, movieId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(response.value);
        }
        [Fact]
        public async Task Update_ReturnsNotFound_WhenItemDoesNotExist()
        {
            _mockMovieService.Setup(service => service.Update(movie))
                .ReturnsAsync(false);
            var result = await _movieController.Update(movie, movieId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(notFoundResult.Value);
            Assert.False(response.value);
            Assert.Equal("Movie couldn't be updated", response.msg);
        }
        [Fact]
        public async Task Update_ReturnsBadRequest_WhenModelIsInvalid()
        {
            _movieController.ModelState.AddModelError("Description", "Required");
            var result = await _movieController.Update(movie, movieId);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(badRequestResult.Value);
            Assert.False(response.status);
            Assert.Contains("Required", response.errors.First());
        }
        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdIsInvalid()
        {
            int movieId = 2;
            var result = await _movieController.Update(movie, movieId);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(badRequestResult.Value);
            Assert.False(response.status);
            Assert.Equal("Movie can't be null or ID mismatch", response.msg);
        }
        [Fact]
        public async Task Update_ReturnsServerError_WhenExceptionThrown()
        {
            _mockMovieService.Setup(service => service.Update(movie))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));
            var result = await _movieController.Update(movie, movieId);

            var serverErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorResult.StatusCode);
            var response = Assert.IsType<Response<bool>>(serverErrorResult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }
    }
}
