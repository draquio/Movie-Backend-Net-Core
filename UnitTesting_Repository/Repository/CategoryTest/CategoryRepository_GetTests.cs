
using Moq;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;

namespace UnitTesting_Repository.Repository.CategoryTest
{
    public class CategoryRepository_GetTests
    {
        private readonly Mock<IGenericRepository<Category>> _mockCategoryRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        int page = 1, pageSize = 10;
        List<Category> categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            };
        public CategoryRepository_GetTests()
        {
            _mockCategoryRepository = new Mock<IGenericRepository<Category>>();
            _categoryRepository = _mockCategoryRepository.Object;
        }

        [Fact]
        public async Task Get_ReturnsListOfItems_WhenItemsExist()
        {

            _mockCategoryRepository.Setup(repo => repo.GetAll(page, pageSize))
                .ReturnsAsync(categories);
            var result = await _categoryRepository.GetAll(page, pageSize);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Id == 1 && c.Name == "Category 1");
            Assert.Contains(result, c => c.Id == 2 && c.Name == "Category 2");
        }
        [Fact]
        public async Task Get_ReturnsEmptyList_WhenNoCategoriesExist()
        {
            _mockCategoryRepository.Setup(repo => repo.GetAll(page, pageSize))
                .ReturnsAsync(new List<Category>());
            var result = await _categoryRepository.GetAll(page, pageSize);
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
