using MovieCRUD_NCapas.DBContext;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;

namespace MovieCRUD_NCapas.Repository
{
    public class MovieActorRepository : IMovieActorRepository
    {
        private readonly DBMovieContext _dbContext;

        public MovieActorRepository(DBMovieContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateMovieActor(MovieActor movieActor)
        {
            _dbContext.Set<MovieActor>().Add(movieActor);
            await _dbContext.SaveChangesAsync();
        }
    }
}
