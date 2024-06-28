using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting.Controllers.Category
{
    public class CategoryController_DeteleTests
    {
        private readonly CategoryController _categoryController;
        private readonly Mock<ICategoryService> _mockCategoryService;
        int categoryId = 1;

        public CategoryController_DeteleTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _categoryController = new CategoryController(_mockCategoryService.Object);
        }
        [Fact]
        public async Task Delete_ReturnsOkResult_WhenDeleteIsSuccessful()
        {
            _mockCategoryService.Setup(service => service.Delete(categoryId))
                .ReturnsAsync(true);
            var result = await _categoryController.Delete(categoryId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(response.status);
        }
        [Fact]
        public async Task Delete_ReturnsNotFound_WhenItemDoesNotExis()
        {
            _mockCategoryService.Setup(service => service.Delete(categoryId))
                .ReturnsAsync(false);
            var result = await _categoryController.Delete(categoryId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(notFoundResult.Value);
            Assert.False(response.status);
            Assert.Equal("Category couldn't be deleted", response.msg);
        }
        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenIdIsInvalid()
        {
            int categoryId = -1;
            var result = await _categoryController.Delete(categoryId);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(notFoundResult.Value);
            Assert.False(response.status);
            Assert.Equal("Category couldn't be deleted", response.msg);
        }
        [Fact]
        public async Task Delete_ReturnsServerError_WhenExceptionThrown()
        {
            _mockCategoryService.Setup(service => service.Delete(categoryId))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));
            var result = await _categoryController.Delete(categoryId);

            var serverErrorREsult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorREsult.StatusCode);
            var response = Assert.IsType<Response<bool>>(serverErrorREsult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }
    }
}
