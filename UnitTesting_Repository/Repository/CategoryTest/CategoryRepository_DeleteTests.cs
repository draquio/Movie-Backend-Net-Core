
using Moq;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;

namespace UnitTesting_Repository.Repository.CategoryTest
{
    public class CategoryRepository_DeleteTests
    {
        private readonly Mock<IGenericRepository<Category>> _mockCategoryRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        Category category = new Category { Id = 1, Name = "Category 1" };

        public CategoryRepository_DeleteTests()
        {
            _mockCategoryRepository = new Mock<IGenericRepository<Category>>();
            _categoryRepository = _mockCategoryRepository.Object;
        }
        [Fact]
        public async Task Delete_ReturnsTrue_WhenDeleteIsSuccessful()
        {
            _mockCategoryRepository.Setup(repo => repo.Delete(category))
                .ReturnsAsync(true);
            var result = await _categoryRepository.Delete(category);
            Assert.True(result);
        }

        [Fact]
        public async Task Delete_ReturnsFalse_WhenDeleteFails()
        {
            var category = new Category { Id = 1, Name = "Thriller" };
            _mockCategoryRepository.Setup(repo => repo.Delete(category))
                .ReturnsAsync(false);
            var result = await _categoryRepository.Delete(category);

            Assert.False(result);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenErrorOccurs()
        {
            _mockCategoryRepository.Setup(repo => repo.Delete(It.IsAny<Category>()))
                .ThrowsAsync(new Exception("Test exception"));

            var exception = await Assert.ThrowsAsync<Exception>(() => _categoryRepository.Delete(category));
            Assert.Equal("Test exception", exception.Message);
        }
    }
}
