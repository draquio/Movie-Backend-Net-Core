using AutoMapper;
using Moq;
using MovieCRUD_NCapas.DTO.Category;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;
using MovieCRUD_NCapas.Services;

namespace UnitTesting.Services.CategoryTest
{
    public class CategoryService_DeleteTests
    {
        private readonly Mock<IGenericRepository<Category>> _mockCategoryRepository;
        private readonly IMapper _mapper;
        private readonly CategoryService _categoryService;
        int categoryId = 1;
        Category existingCategory = new Category { Id = 1, Name = "Existing Category" };
        public CategoryService_DeleteTests()
        {
            _mockCategoryRepository = new Mock<IGenericRepository<Category>>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryDTO>().ReverseMap();
            });
            _mapper = config.CreateMapper();

            _categoryService = new CategoryService(_mockCategoryRepository.Object, _mapper);
        }

        [Fact]
        public async Task Delete_ReturnsTrue_WhenDeleteIsSuccessful()
        {
            _mockCategoryRepository.Setup(repo => repo.GetById(categoryId))
                .ReturnsAsync(existingCategory);
            _mockCategoryRepository.Setup(repo => repo.Delete(existingCategory))
                .ReturnsAsync(true);
            var result = await _categoryService.Delete(categoryId);
            Assert.True(result);
        }
        [Fact]
        public async Task Delete_ThrowsApplicationException_WhenItemDoesNotExist()
        {
            _mockCategoryRepository.Setup(repo => repo.GetById(categoryId))
                .ReturnsAsync((Category)null);

            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _categoryService.Delete(categoryId));
            Assert.Contains($"An error occurred while deleting the category: Category with ID {categoryId} not found", exception.Message);
        }

        [Fact]
        public async Task Delete_ThrowsApplicationException_WhenDeleteFails()
        {
            _mockCategoryRepository.Setup(repo => repo.GetById(categoryId))
                .ReturnsAsync(existingCategory);
            _mockCategoryRepository.Setup(repo => repo.Delete(existingCategory))
                .ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _categoryService.Delete(categoryId));
            Assert.Contains("An error occurred while deleting the category: Category couldn't be deleted", exception.Message);
        }

        [Fact]
        public async Task Delete_ThrowsApplicationException_WhenErrorOccurs()
        {
            _mockCategoryRepository.Setup(repo => repo.GetById(categoryId))
                .ReturnsAsync(existingCategory);
            _mockCategoryRepository.Setup(repo => repo.Delete(existingCategory))
                .ThrowsAsync(new Exception("Test exception"));

            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _categoryService.Delete(categoryId));
            Assert.Contains("An error occurred while deleting the category: Test exception", exception.Message);
        }
    }
}
