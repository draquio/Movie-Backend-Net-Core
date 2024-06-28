
using AutoMapper;
using Moq;
using MovieCRUD_NCapas.DTO.Category;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;
using MovieCRUD_NCapas.Services;

namespace UnitTesting.Services.CategoryTest
{
    public class CategoryService_GetByIdTests
    {
        private readonly Mock<IGenericRepository<Category>> _mockCategoryRepository;
        private readonly IMapper _mapper;
        private readonly CategoryService _categoryService;
        int categoryId = 1;
        public CategoryService_GetByIdTests()
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
        public async Task GetById_ReturnsItem_WhenItemExists()
        {
            Category category = new Category { Id = categoryId, Name = "Category 1" };
            _mockCategoryRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(category);
            var result = await _categoryService.GetById(categoryId);
            Assert.NotNull(result);
            Assert.Equal(categoryId, result.Id);
            Assert.Equal("Category 1", result.Name);
        }
        [Fact]
        public async Task GetById_ReturnsNull_WhenItemDoesNotExist()
        {
            _mockCategoryRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync((Category)null);

            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _categoryService.GetById(categoryId));
            Assert.Contains($"An error occurred while retrieving the category: Category with ID {categoryId} not found", exception.Message);
        }
        [Fact]
        public async Task GetById_ThrowsException_WhenErrorOccurs()
        {
            _mockCategoryRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));
            await Assert.ThrowsAsync<ApplicationException>(() => _categoryService.GetById(categoryId));
        }
    }
}
