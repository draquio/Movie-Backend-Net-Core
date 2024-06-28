using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.DTO.Review;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting.Controllers.Review
{
    public class ReviewController_GetByIdTests
    {
        private readonly ReviewController _reviewController;
        private readonly Mock<IReviewService> _mockReviewService;
        public ReviewController_GetByIdTests()
        {
            {
                _mockReviewService = new Mock<IReviewService>();
                _reviewController = new ReviewController(_mockReviewService.Object);
            }
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithItem()
        {
            int reviewId = 1;
            var review = new ReviewDTO { Id = 1, MovieId = 1, MovieName = "pelicula 1" };
            _mockReviewService.Setup(service => service.GetById(reviewId))
                .ReturnsAsync(review);
            var result = await _reviewController.GetReview(reviewId);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<ReviewDTO>>(okResult.Value);
            Assert.Equal(reviewId, response.value.Id);
            _mockReviewService.Verify(service => service.GetById(reviewId), Times.Once());
        }
        [Fact]
        public async Task GetById_ReturnsNotFound_WhenItemDoesNotExist()
        {
            int reviewId = 1;
            _mockReviewService.Setup(service => service.GetById(reviewId))
                .ReturnsAsync((ReviewDTO)null);
            var result = await _reviewController.GetReview(reviewId);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = Assert.IsType<Response<ReviewDTO>>(notFoundResult.Value);
            Assert.False(response.status);
            Assert.Equal($"Review with ID {reviewId} not found", response.msg);
        }
        [Fact]
        public async Task GetById_ThrowsException_WhenIdIsInvalid()
        {
            int reviewId = -1;
            _mockReviewService.Setup(service => service.GetById(reviewId))
                .ThrowsAsync(new ArgumentException("Invalid ID"));
            var result = await _reviewController.GetReview(reviewId);

            var badRequestResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, badRequestResult.StatusCode);
            var response = Assert.IsType<Response<ReviewDTO>>(badRequestResult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Invalid ID", response.msg);
        }
        [Fact]
        public async Task GetById_ReturnsServerError_WhenExceptionThrown()
        {
            int reviewId = 1;
            _mockReviewService.Setup(service => service.GetById(reviewId))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));
            var result = await _reviewController.GetReview(reviewId);

            var serverErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorResult.StatusCode);
            var response = Assert.IsType<Response<ReviewDTO>>(serverErrorResult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }
    }
}
