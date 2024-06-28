using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.DTO.Review;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting.Controllers.Review
{
    public class ReviewController_GetTests
    {
        private readonly ReviewController _reviewController;
        private readonly Mock<IReviewService> _mockReviewService;
        int page = 1, pageSize = 10;
        public ReviewController_GetTests()
        {
            _mockReviewService = new Mock<IReviewService>();
            _reviewController = new ReviewController(_mockReviewService.Object);
        }

        [Fact]
        public async Task GetList_ReturnsOkResult_WithListofReviews()
        {
            _mockReviewService.Setup(service => service.GetReviews(page, pageSize))
            .ReturnsAsync(new List<ReviewDTO>
            {
                    new ReviewDTO { Id = 1, MovieId = 1, MovieName = "pelicula 1" },
                    new ReviewDTO { Id = 2, MovieId = 2, MovieName = "pelicula 2" },
                    new ReviewDTO { Id = 3, MovieId = 3, MovieName = "pelicula 3" }
            });
            var result = await _reviewController.GetList(page, pageSize); // hacemos la peticion
            var okResult = Assert.IsType<OkObjectResult>(result.Result); // convertimo de ActionResult a OkObjectResult
            var response = Assert.IsType<Response<List<ReviewDTO>>>(okResult.Value); // Optenemos el valor del Response
            var reviews = response.value; // en reviews sacamos el valor de la lista que esta dentro de Response -> value
            Assert.Equal(3, reviews.Count); // confirmamos que sean 3 por el mock que hicimos
        }
        [Fact]
        public async Task GetList_ReturnsOkResult_WhenListIsEmpty()
        {
            _mockReviewService.Setup(service => service.GetReviews(page, pageSize))
                .ReturnsAsync(new List<ReviewDTO>());
            var result = await _reviewController.GetList(page, pageSize);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<List<ReviewDTO>>>(okResult.Value);
            Assert.Empty(response.value);
        }
        [Fact]
        public async Task GetList_ReturnsServerError_WhenExceptionThrown()
        {
            _mockReviewService.Setup(service => service.GetReviews(page, pageSize))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));

            var result = await _reviewController.GetList(page, pageSize);
            var serverErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorResult.StatusCode);
            var response = Assert.IsType<Response<List<ReviewDTO>>>(serverErrorResult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }

    }
}
