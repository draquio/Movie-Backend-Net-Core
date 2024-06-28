using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.DTO.Review;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting.Controllers.Review
{
    public  class ReviewController_CreateTests
    {
        private readonly ReviewController _reviewController;
        private readonly Mock<IReviewService> _mockReviewService;
        ReviewDTO newReviewDTO = new ReviewDTO { Id = 1, MovieId = 1, Comment = "buena pelicula", Rating = "3/5" };
        ReviewDTO createdReview = new ReviewDTO { Id = 1, MovieId = 1, Comment = "buena pelicula", Rating = "3/5" };
        public ReviewController_CreateTests()
        {
            _mockReviewService = new Mock<IReviewService>();
            _reviewController = new ReviewController(_mockReviewService.Object);
        }

        [Fact]
        public async Task Create_ReturnsOkResult_WithCreatedItem()
        {

            _mockReviewService.Setup(service => service.Create(newReviewDTO))
                .ReturnsAsync(createdReview);
            var result = await _reviewController.Create(newReviewDTO);

            var okResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var response = Assert.IsType<Response<ReviewDTO>>(okResult.Value);
            Assert.Equal(createdReview.Id, response.value.Id);
        }
        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelIsInvalid()
        {
            _reviewController.ModelState.AddModelError("Comment", "Required");
            ReviewDTO newReview = new ReviewDTO { MovieId = 1 };
            var result = await _reviewController.Create(newReview);

            var badRequetResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var response = Assert.IsType<Response<ReviewDTO>>(badRequetResult.Value);
            Assert.False(response.status);
            Assert.Contains("Required", response.errors.First());
        }
        [Fact]
        public async Task Create_ReturnsServerError_WhenExceptionThrown()
        {
            _mockReviewService.Setup(service => service.Create(newReviewDTO))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));
            var result = await _reviewController.Create(newReviewDTO);

            var serverErrorresult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorresult.StatusCode);
            var response = Assert.IsType<Response<ReviewDTO>>(serverErrorresult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }
    }
}
