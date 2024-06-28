

using AutoMapper;
using Moq;
using MovieCRUD_NCapas.DTO.Category;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;
using MovieCRUD_NCapas.Services;

namespace UnitTesting.Services.CategoryTest
{
    public class CategoryService_CreateTests
    {
        private readonly Mock<IGenericRepository<Category>> _mockCategoryRepository;
        private readonly IMapper _mapper;
        private readonly CategoryService _categoryService;
        CategoryDTO categoryDto = new CategoryDTO { Id = 1, Name = "New Category" };
        Category createdCategory = new Category { Id = 1, Name = "New Category" };
        public CategoryService_CreateTests()
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
        public async Task Create_ReturnsCreatedItem()
        {
            _mockCategoryRepository.Setup(repo => repo.Create(It.IsAny<Category>()))
                .ReturnsAsync(createdCategory);
            var result = await _categoryService.Create(categoryDto);

            Assert.NotNull(result);
            Assert.Equal(createdCategory.Id, result.Id);
            Assert.Equal(createdCategory.Name, result.Name);
        }
        [Fact]
        public async Task Create_ThrowsException_WhenErrorOccurs()
        {
            _mockCategoryRepository.Setup(repo => repo.Create(It.IsAny<Category>()))
                .ThrowsAsync(new InvalidOperationException("Category couldn't be created"));

            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _categoryService.Create(categoryDto));
            Assert.Contains("An error occurred while creating the category: Category couldn't be created", exception.Message);
        }
    }
}
