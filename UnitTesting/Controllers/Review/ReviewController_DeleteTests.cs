using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting.Controllers.Review
{
    public class ReviewController_DeleteTests
    {
        private readonly ReviewController _reviewController;
        private readonly Mock<IReviewService> _mockReviewService;
        int reviewId = 1;
        public ReviewController_DeleteTests()
        {
            _mockReviewService = new Mock<IReviewService>();
            _reviewController = new ReviewController(_mockReviewService.Object);
        }
        [Fact]
        public async Task Delete_ReturnsOkResult_WhenDeleteIsSuccessful()
        {
            _mockReviewService.Setup(service => service.Delete(reviewId))
                .ReturnsAsync(true);
            var result = await _reviewController.Delete(reviewId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(response.status);
        }
        [Fact]
        public async Task Delete_ReturnsNotFound_WhenItemDoesNotExis()
        {
            _mockReviewService.Setup(service => service.Delete(reviewId))
                .ReturnsAsync(false);
            var result = await _reviewController.Delete(reviewId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(notFoundResult.Value);
            Assert.False(response.status);
            Assert.Equal("Review couldn't be deleted", response.msg);
        }
        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenIdIsInvalid()
        {
            int reviewId = -1;
            var result = await _reviewController.Delete(reviewId);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(notFoundResult.Value);
            Assert.False(response.status);
            Assert.Equal("Review couldn't be deleted", response.msg);
        }
        [Fact]
        public async Task Delete_ReturnsServerError_WhenExceptionThrown()
        {
            _mockReviewService.Setup(service => service.Delete(reviewId))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));
            var result = await _reviewController.Delete(reviewId);

            var serverErrorREsult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorREsult.StatusCode);
            var response = Assert.IsType<Response<bool>>(serverErrorREsult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }
    }
}
