using MovieCRUD_NCapas.DTO.Category;

namespace MovieCRUD_NCapas.Services.Interface
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetCategories();
        Task<CategoryDTO> GetById(int id);
        Task<CategoryDTO> Create(CategoryDTO categoryDTO);
        Task<bool> Update(CategoryDTO categoryDTO);
        Task<bool> Delete(int id);
    }
}
