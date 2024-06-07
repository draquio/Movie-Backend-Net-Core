using MovieCRUD_NCapas.DTO;

namespace MovieCRUD_NCapas.Services.Interface
{
    public interface IMovieService
    {
        Task<List<MovieDTO>> GetMovies();
        Task<MovieDTO> GetById(int id);
        Task<MovieDTO> Create(MovieDTO movieDTO);
        Task<bool> Update(MovieDTO movieDTO);
        Task<bool> Delete(int id);
    }
}
