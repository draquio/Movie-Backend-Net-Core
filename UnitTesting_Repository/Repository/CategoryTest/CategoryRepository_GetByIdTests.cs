
using Moq;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;

namespace UnitTesting_Repository.Repository.CategoryTest
{
    public class CategoryRepository_GetByIdTests
    {
        private readonly Mock<IGenericRepository<Category>> _mockCategoryRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        int categoryId = 1;
        Category category = new Category { Id = 1, Name = "Category 1" };
        public CategoryRepository_GetByIdTests()
        {
            _mockCategoryRepository = new Mock<IGenericRepository<Category>>();
            _categoryRepository = _mockCategoryRepository.Object;
        }
        [Fact]
        public async Task GetById_ReturnsItem_WhenItemExists()
        {
            _mockCategoryRepository.Setup(repo => repo.GetById(categoryId))
                .ReturnsAsync(category);
            var result = await _categoryRepository.GetById(categoryId);

            Assert.NotNull(result);
            Assert.Equal(category.Id, result.Id);
            Assert.Equal(category.Name, result.Name);
        }

        [Fact]
        public async Task GetById_ReturnsNull_WhenItemDoesNotExist()
        {
            int categoryId = 2;
            _mockCategoryRepository.Setup(repo => repo.GetById(categoryId))
                .ReturnsAsync((Category)null);
            var result = await _categoryRepository.GetById(categoryId);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetById_ThrowsException_WhenErrorOccurs()
        {
            _mockCategoryRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));
            var exception = await Assert.ThrowsAsync<Exception>(() => _categoryRepository.GetById(categoryId));
            Assert.Equal("Test exception", exception.Message);
        }
    }
}
