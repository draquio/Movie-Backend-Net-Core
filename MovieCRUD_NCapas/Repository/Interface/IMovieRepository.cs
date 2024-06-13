using MovieCRUD_NCapas.Models;

namespace MovieCRUD_NCapas.Repository.Interface
{
    public interface IMovieRepository : IGenericRepository<Movie>
    {
        Task<Movie> CreateMovie(Movie movie, List<int> actorsIds, List<int> categoriesIds);
        Task<List<Movie>> GetAllMovies(int page, int pageSize);
    }
}
