using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.DTO.Review;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting.Controllers.Review
{
    public class ReviewController_UpdateTests
    {
        private readonly ReviewController _reviewController;
        private readonly Mock<IReviewService> _mockReviewService;
        int reviewId = 1;
        ReviewDTO review = new ReviewDTO { Id = 1, Comment = "buena película" };
        public ReviewController_UpdateTests()
        {
            _mockReviewService = new Mock<IReviewService>();
            _reviewController = new ReviewController(_mockReviewService.Object);
        }
        [Fact]
        public async Task Update_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            _mockReviewService.Setup(service => service.Update(review))
                .ReturnsAsync(true);
            var result = await _reviewController.Update(review, reviewId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(response.value);
        }
        [Fact]
        public async Task Update_ReturnsNotFound_WhenItemDoesNotExist()
        {
            _mockReviewService.Setup(service => service.Update(review))
                .ReturnsAsync(false);
            var result = await _reviewController.Update(review, reviewId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response  = Assert.IsType<Response<bool>>(notFoundResult.Value);
            Assert.False(response.value);
            Assert.Equal("Review couldn't be updated", response.msg);
        }
        [Fact]
        public async Task Update_ReturnsBadRequest_WhenModelIsInvalid()
        {
            _reviewController.ModelState.AddModelError("Comment", "Required");
            var result = await _reviewController.Update(review, reviewId);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(badRequestResult.Value);
            Assert.False(response.status);
            Assert.Contains("Required", response.errors.First());
        }
        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdIsInvalid()
        {
            int reviewId = 2;
            var result = await _reviewController.Update(review, reviewId);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(badRequestResult.Value);
            Assert.False(response.status);
            Assert.Equal("Review can't be null or ID mismatch", response.msg);
        }
        [Fact]
        public async Task Update_ReturnsServerError_WhenExceptionThrown()
        {
            _mockReviewService.Setup(service => service.Update(review))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));
            var result = await _reviewController.Update(review, reviewId);

            var serverErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorResult.StatusCode);
            var response = Assert.IsType<Response<bool>>(serverErrorResult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }
    }
}
