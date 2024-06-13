using MovieCRUD_NCapas.DTO.Movie;

namespace MovieCRUD_NCapas.Services.Interface
{
    public interface IMovieService
    {
        Task<List<MovieDTO>> GetMovies(int page, int pageSize);
        Task<MovieDTO> GetById(int id);
        Task<MovieDTO> Create(CreateMovieDTO movieDTO);
        Task<bool> Update(MovieDTO movieDTO);
        Task<bool> Delete(int id);
    }
}
