using AutoMapper;
using MovieCRUD_NCapas.DTO.Category;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;
using MovieCRUD_NCapas.Services.Interface;

namespace MovieCRUD_NCapas.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IGenericRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        // Get category list
        public async Task<List<CategoryDTO>> GetCategories()
        {
            try
            {
                List<Category> listCategories = await _categoryRepository.GetAll();
                if (listCategories == null || !listCategories.Any())
                {
                    return new List<CategoryDTO>();
                }
                List<CategoryDTO> listCategoriesDTO = _mapper.Map<List<CategoryDTO>>(listCategories);
                return listCategoriesDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving the categories: {ex.Message}", ex);
            }
        }

        // Get a category by Id
        public async Task<CategoryDTO> GetById(int id)
        {
            try
            {
                Category categorySearched = await _categoryRepository.GetById(id);
                if (categorySearched == null)
                {
                    throw new KeyNotFoundException($"Category with ID {id} not found");
                }
                CategoryDTO categoryDTO = _mapper.Map<CategoryDTO>(categorySearched);
                return categoryDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving the category: {ex.Message}", ex);
            }
        }

        // Create a category
        public async Task<CategoryDTO> Create(CategoryDTO categoryDTO)
        {
            try
            {
                Category category = _mapper.Map<Category>(categoryDTO);
                Category createdCategory = await _categoryRepository.Create(category);
                if (createdCategory == null || createdCategory.Id == 0)
                {
                    throw new InvalidOperationException("Category couldn't be created");
                }
                CategoryDTO createdCategoryDTO = _mapper.Map<CategoryDTO>(createdCategory);
                return createdCategoryDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while creating the category: {ex.Message}", ex);
            }
        }
        
        // Update a category
        public async Task<bool> Update(CategoryDTO categoryDTO)
        {
            try
            {
                Category categorySearched = await _categoryRepository.GetById(categoryDTO.Id);
                if (categorySearched == null)
                {
                    throw new KeyNotFoundException($"Category with ID {categoryDTO.Id} not found");
                }
                _mapper.Map(categoryDTO, categorySearched);
                bool response = await _categoryRepository.Update(categorySearched);
                if (!response)
                {
                    throw new InvalidOperationException("Category couldn't be updated");
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while updating the category: {ex.Message}", ex);
            }
        }

        // Delete a category by Id
        public async Task<bool> Delete(int id)
        {
            try
            {
                Category categorySearched = await _categoryRepository.GetById(id);
                if (categorySearched == null)
                {
                    throw new KeyNotFoundException($"Category with ID {id} not found");
                }
                bool response = await _categoryRepository.Delete(categorySearched);
                if (!response)
                {
                    throw new InvalidOperationException("Category couldn't be deleted");
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting the category: {ex.Message}", ex);
            }
        }
    }
}
