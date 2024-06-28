using AutoMapper;
using Moq;
using MovieCRUD_NCapas.DTO.Category;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;
using MovieCRUD_NCapas.Services;

namespace UnitTesting.Services.CategoryTest
{
    public class CategoryService_GetTests
    {
        private readonly Mock<IGenericRepository<Category>> _mockCategoryRepository;
        private readonly IMapper _mapper;
        private readonly CategoryService _categoryService;

        int page = 1, pageSize = 10;

        public CategoryService_GetTests()
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
        public async Task GetList_ReturnsListOfItems()
        {
            List<Category> categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            };
            _mockCategoryRepository.Setup(repo => repo.GetAll(page, pageSize))
                .ReturnsAsync(categories);
            var result = await _categoryService.GetCategories(page, pageSize);

            Assert.Equal(2, result.Count);
            Assert.Equal("Category 1", result[0].Name);
            Assert.Equal("Category 2", result[1].Name);
        }
        [Fact]
        public async Task GetList_ReturnsEmptyList_WhenNoItemsExist()
        {
            _mockCategoryRepository.Setup(repo => repo.GetAll(page, pageSize))
                .ReturnsAsync(new List<Category>());
            var result = await _categoryService.GetCategories(page, pageSize);
            Assert.Empty(result);
        }
        [Fact]
        public async Task GetList_ThrowsException_WhenErrorOccurs()
        {
            _mockCategoryRepository.Setup(repo => repo.GetAll(page, pageSize))
                .ThrowsAsync(new Exception("Test exception"));
            await Assert.ThrowsAsync<ApplicationException>(() => _categoryService.GetCategories(page, pageSize));
        }
    }
}
