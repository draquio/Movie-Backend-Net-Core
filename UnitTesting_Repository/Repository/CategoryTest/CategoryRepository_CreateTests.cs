using Moq;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting_Repository.Repository.CategoryTest
{
    public class CategoryRepository_CreateTests
    {
        private readonly Mock<IGenericRepository<Category>> _mockCategoryRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        Category category = new Category { Id = 1, Name = "Category 1" };

        public CategoryRepository_CreateTests()
        {
            _mockCategoryRepository = new Mock<IGenericRepository<Category>>();
            _categoryRepository = _mockCategoryRepository.Object;
        }
        [Fact]
        public async Task Create_ReturnsOkResult_WithCreatedItem()
        {
            _mockCategoryRepository.Setup(repo => repo.Create(It.IsAny<Category>()))
                .ReturnsAsync(category);
            var result = await _categoryRepository.Create(category);
            Assert.NotNull(result);
            Assert.Equal(category.Name, result.Name);
        }

        [Fact]
        public async Task Create_ReturnsNull_WhenItemIsInvalid()
        {
            Category category = null;
            _mockCategoryRepository.Setup(repo => repo.Create(It.IsAny<Category>()))
                .ReturnsAsync((Category)null);

            var result = await _categoryRepository.Create(category);

            Assert.Null(result);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenErrorOccurs()
        {
            _mockCategoryRepository.Setup(repo => repo.Create(It.IsAny<Category>()))
                .ThrowsAsync(new Exception("Test exception"));
            var exception = await Assert.ThrowsAsync<Exception>(() => _categoryRepository.Create(category));
            Assert.Equal("Test exception", exception.Message);
        }
    }
}
