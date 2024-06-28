using MovieCRUD_NCapas.Models;

namespace MovieCRUD_NCapas.Repository.Interface
{
    public interface IMovieRepository : IGenericRepository<Movie>
    {
        Task<Movie> Create(Movie movie, List<int> actorsIds, List<int> categoriesIds);
        Task<List<Movie>> GetAll(int page, int pageSize);
    }
}
