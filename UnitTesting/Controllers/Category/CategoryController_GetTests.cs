using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.DTO.Category;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting.Controllers.Category
{
    public class CategoryController_GetTests
    {
        private readonly CategoryController _categoryController;
        private readonly Mock<ICategoryService> _mockCategoryService;
        int page = 1, pageSize = 10;

        public CategoryController_GetTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _categoryController = new CategoryController(_mockCategoryService.Object);
        }

        [Fact]
        public async Task GetList_ReturnsOkResult_WithListofItems()
        {
            _mockCategoryService.Setup(service => service.GetCategories(page, pageSize))
            .ReturnsAsync(new List<CategoryDTO>
            {
                    new CategoryDTO { Id = 1, Name = "Categoria 1" },
                    new CategoryDTO { Id = 2, Name = "Categoria 2" },
                    new CategoryDTO { Id = 2, Name = "Categoria 3" },
            });
            var result = await _categoryController.GetList(page, pageSize);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<List<CategoryDTO>>>(okResult.Value);
            var categories = response.value;
            Assert.Equal(3, categories.Count);
        }
        [Fact]
        public async Task GetList_ReturnsOkResult_WhenListIsEmpty()
        {
            _mockCategoryService.Setup(service => service.GetCategories(page, pageSize))
                .ReturnsAsync(new List<CategoryDTO>());
            var result = await _categoryController.GetList(page, pageSize);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<List<CategoryDTO>>>(okResult.Value);
            Assert.Empty(response.value);
        }
        [Fact]
        public async Task GetList_ReturnsServerError_WhenExceptionThrown()
        {
            _mockCategoryService.Setup(service => service.GetCategories(page, pageSize))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));

            var result = await _categoryController.GetList(page, pageSize);
            var serverErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorResult.StatusCode);
            var response = Assert.IsType<Response<List<CategoryDTO>>>(serverErrorResult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }
    }
}
