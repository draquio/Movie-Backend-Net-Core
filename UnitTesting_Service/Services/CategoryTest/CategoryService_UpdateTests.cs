using AutoMapper;
using Moq;
using MovieCRUD_NCapas.DTO.Category;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;
using MovieCRUD_NCapas.Services;

namespace UnitTesting.Services.CategoryTest
{
    public class CategoryService_UpdateTests
    {
        private readonly Mock<IGenericRepository<Category>> _mockCategoryRepository;
        private readonly IMapper _mapper;
        private readonly CategoryService _categoryService;
        CategoryDTO categoryDto = new CategoryDTO { Id = 1, Name = "Updated category" };
        Category existingCategory = new Category { Id = 1, Name = "Existing category" };
        public CategoryService_UpdateTests()
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
        public async Task Update_ReturnsTrue_WhenUpdateIsSuccessful()
        {
            _mockCategoryRepository.Setup(repo => repo.GetById(categoryDto.Id))
                .ReturnsAsync(existingCategory);
            _mockCategoryRepository.Setup(repo => repo.Update(It.IsAny<Category>()))
                .ReturnsAsync(true);
            var result = await _categoryService.Update(categoryDto);

            Assert.True(result);
        }
        [Fact]
        public async Task Update_ReturnsFalse_WhenItemDoesNotExist()
        {
            _mockCategoryRepository.Setup(repo => repo.GetById(categoryDto.Id))
                .ReturnsAsync((Category)null);
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _categoryService.Update(categoryDto));
            Assert.Contains($"An error occurred while updating the category: Category with ID {categoryDto.Id} not found", exception.Message);
        }
        [Fact]
        public async Task Update_ThrowsException_WhenErrorOccurs()
        {
            _mockCategoryRepository.Setup(repo => repo.GetById(categoryDto.Id))
                .ReturnsAsync(existingCategory);
            _mockCategoryRepository.Setup(repo => repo.Update(It.IsAny<Category>()))
                .ThrowsAsync(new Exception("Test exception"));

            await Assert.ThrowsAsync<ApplicationException>(() => _categoryService.Update(categoryDto));
        }
    }
}
