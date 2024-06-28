using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.DTO.Category;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting.Controllers.Category
{
    public class CategoryController_UpdateTests
    {
        private readonly CategoryController _categoryController;
        private readonly Mock<ICategoryService> _mockCategoryService;
        int categoryId = 1;
        CategoryDTO category = new CategoryDTO { Id = 1, Name = "Categoría 1" };
        public CategoryController_UpdateTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _categoryController = new CategoryController(_mockCategoryService.Object);
        }
        [Fact]
        public async Task Update_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            _mockCategoryService.Setup(service => service.Update(category))
                .ReturnsAsync(true);
            var result = await _categoryController.Update(category, categoryId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(response.value);
        }
        [Fact]
        public async Task Update_ReturnsNotFound_WhenItemDoesNotExist()
        {
            _mockCategoryService.Setup(service => service.Update(category))
                .ReturnsAsync(false);
            var result = await _categoryController.Update(category, categoryId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(notFoundResult.Value);
            Assert.False(response.value);
            Assert.Equal("Category couldn't be updated", response.msg);
        }
        [Fact]
        public async Task Update_ReturnsBadRequest_WhenModelIsInvalid()
        {
            _categoryController.ModelState.AddModelError("Description", "Required");
            var result = await _categoryController.Update(category, categoryId);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(badRequestResult.Value);
            Assert.False(response.status);
            Assert.Contains("Required", response.errors.First());
        }
        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdIsInvalid()
        {
            int categoryId = 2;
            var result = await _categoryController.Update(category, categoryId);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var response = Assert.IsType<Response<bool>>(badRequestResult.Value);
            Assert.False(response.status);
            Assert.Equal("Category can't be null or ID mismatch", response.msg);
        }
        [Fact]
        public async Task Update_ReturnsServerError_WhenExceptionThrown()
        {
            _mockCategoryService.Setup(service => service.Update(category))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));
            var result = await _categoryController.Update(category, categoryId);

            var serverErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorResult.StatusCode);
            var response = Assert.IsType<Response<bool>>(serverErrorResult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }
    }
}
