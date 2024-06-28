using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.DTO.Category;
using MovieCRUD_NCapas.DTO.Movie;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.Controllers.Movie
{
    public class MovieController_GetByIdTests
    {
        private readonly MovieController _movieController;
        private readonly Mock<IMovieService> _mockMovieService;
        int movieId = 1;

        public MovieController_GetByIdTests()
        {
            _mockMovieService = new Mock<IMovieService>();
            _movieController = new MovieController(_mockMovieService.Object);
        }
        [Fact]
        public async Task GetById_ReturnsOkResult_WithItem()
        {
            MovieDTO movie = new MovieDTO { Id = 1, Title = "Movie 1" };
            _mockMovieService.Setup(service => service.GetById(movieId))
                .ReturnsAsync(movie);
            var result = await _movieController.GetMovie(movieId);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<MovieDTO>>(okResult.Value);
            Assert.Equal(movieId, response.value.Id);
        }
        [Fact]
        public async Task GetById_ReturnsNotFound_WhenItemDoesNotExist()
        {
            _mockMovieService.Setup(service => service.GetById(movieId))
                .ReturnsAsync((MovieDTO)null);
            var result = await _movieController.GetMovie(movieId);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = Assert.IsType<Response<MovieDTO>>(notFoundResult.Value);
            Assert.False(response.status);
            Assert.Equal($"Movie with ID {movieId} not found", response.msg);
        }
        [Fact]
        public async Task GetById_ThrowsException_WhenIdIsInvalid()
        {
            int movieId = -1;
            _mockMovieService.Setup(service => service.GetById(movieId))
                .ThrowsAsync(new ArgumentException("Invalid ID"));
            var result = await _movieController.GetMovie(movieId);

            var badRequestResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, badRequestResult.StatusCode);
            var response = Assert.IsType<Response<MovieDTO>>(badRequestResult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Invalid ID", response.msg);
        }
        [Fact]
        public async Task GetById_ReturnsServerError_WhenExceptionThrown()
        {
            _mockMovieService.Setup(service => service.GetById(movieId))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));
            var result = await _movieController.GetMovie(movieId);

            var serverErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorResult.StatusCode);
            var response = Assert.IsType<Response<MovieDTO>>(serverErrorResult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }
    }
}
