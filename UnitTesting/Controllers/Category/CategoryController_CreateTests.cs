using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCRUD_NCapas.Controllers;
using MovieCRUD_NCapas.DTO.Category;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting.Controllers.Category
{
    public class CategoryController_CreateTests
    {
        private readonly CategoryController _categoryController;
        private readonly Mock<ICategoryService> _mockCategoryService;
        CategoryDTO newCategoryDTO = new CategoryDTO { Id = 1, Name = "Categoría 1"};
        CategoryDTO createdCategory = new CategoryDTO { Id = 1, Name = "Categoría 1" };
        public CategoryController_CreateTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _categoryController = new CategoryController(_mockCategoryService.Object);
        }
        [Fact]
        public async Task Create_ReturnsOkResult_WithCreatedItem()
        {
            _mockCategoryService.Setup(service => service.Create(newCategoryDTO))
                .ReturnsAsync(createdCategory);
            var result = await _categoryController.Create(newCategoryDTO);

            var okResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var response = Assert.IsType<Response<CategoryDTO>>(okResult.Value);
            Assert.Equal(createdCategory.Id, response.value.Id);
        }
        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelIsInvalid()
        {
            _categoryController.ModelState.AddModelError("Description", "Required");
            var result = await _categoryController.Create(newCategoryDTO);

            var badRequetResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var response = Assert.IsType<Response<CategoryDTO>>(badRequetResult.Value);
            Assert.False(response.status);
            Assert.Contains("Required", response.errors.First());
        }
        [Fact]
        public async Task Create_ReturnsServerError_WhenExceptionThrown()
        {
            _mockCategoryService.Setup(service => service.Create(newCategoryDTO))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));
            var result = await _categoryController.Create(newCategoryDTO);

            var serverErrorresult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverErrorresult.StatusCode);
            var response = Assert.IsType<Response<CategoryDTO>>(serverErrorresult.Value);
            Assert.False(response.status);
            Assert.Equal("An error occurred: Simulated database error", response.msg);
        }
    }
}
