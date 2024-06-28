using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.DTO.Category;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;


namespace UnitTesting.Controllers.Category
{
    public class CategoryController_GetByIdTests
    {
        private readonly CategoryController _categoryController;
        private readonly Mock<ICategoryService> _mockCategoryService;
        int categoryId = 1;

        public CategoryController_GetByIdTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _categoryController = new CategoryController(_mockCategoryService.Object);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithItem()
        {
            var category = new CategoryDTO { Id = 1, Name = "Categoría 1" };
            _mockCategoryService.Setup(service => service.GetById(categoryId))
                .ReturnsAsync(category);
            var result = await _categoryController.GetCategory(categoryId);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<CategoryDTO>>(okResult.Value);
            Assert.Equal(categoryId, response.value.Id);
        }
        [Fact]
        public async Task GetById_ReturnsNotFound_WhenItemDoesNotExist()
        {
            _mockCategoryService.Setup(service => service.GetById(categoryId))
                .ReturnsAsync((CategoryDTO)null);
            var result = await _categoryController.GetCategory(categoryId);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = Assert.IsType<Response<CategoryDTO>>(notFoundResult.Value);
            Assert.False(response.status);
            Assert.Equal($"Category with ID {categoryId} not found", response.msg);
        }
        [Fact]
        public async Task GetById_ThrowsException_WhenIdIsInvalid()
        {
            int categoryId = -1;
            _mockCategoryService.Setup(service => service.GetById(categoryId))
                .ThrowsAsync(new ArgumentException("Invalid ID"));
            var result = await _categoryController.GetCategory(categoryId);

            var badRequestResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, badRequestResult.StatusCode);
            var response = Assert.IsType<Response<CategoryDTO>>(badRequestResult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Invalid ID", response.msg);
        }
        [Fact]
        public async Task GetById_ReturnsServerError_WhenExceptionThrown()
        {
            _mockCategoryService.Setup(service => service.GetById(categoryId))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));
            var result = await _categoryController.GetCategory(categoryId);

            var serverErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorResult.StatusCode);
            var response = Assert.IsType<Response<CategoryDTO>>(serverErrorResult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }
    }
}
