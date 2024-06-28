

using Moq;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;

namespace UnitTesting_Repository.Repository.CategoryTest
{
    public class CategoryRepository_UpdateTests
    {
        private readonly Mock<IGenericRepository<Category>> _mockCategoryRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        Category category = new Category { Id = 1, Name = "Category 1" };

        public CategoryRepository_UpdateTests()
        {
            _mockCategoryRepository = new Mock<IGenericRepository<Category>>();
            _categoryRepository = _mockCategoryRepository.Object;
        }
        [Fact]
        public async Task Update_ReturnsTrue_WhenUpdateIsSuccessful()
        {
            _mockCategoryRepository.Setup(repo => repo.Update(It.IsAny<Category>()))
                .ReturnsAsync(true);
            var result = await _categoryRepository.Update(category);

            Assert.True(result);
        }

        [Fact]
        public async Task Update_ReturnsFalse_WhenUpdateFails()
        {
            _mockCategoryRepository.Setup(repo => repo.Update(It.IsAny<Category>()))
                .ReturnsAsync(false);
            var result = await _categoryRepository.Update(category);

            Assert.False(result);
        }

        [Fact]
        public async Task Update_ThrowsException_WhenErrorOccurs()
        {
            _mockCategoryRepository.Setup(repo => repo.Update(It.IsAny<Category>()))
                .ThrowsAsync(new Exception("Test exception"));

            var exception = await Assert.ThrowsAsync<Exception>(() => _categoryRepository.Update(category));
            Assert.Equal("Test exception", exception.Message);
        }
    }
}
