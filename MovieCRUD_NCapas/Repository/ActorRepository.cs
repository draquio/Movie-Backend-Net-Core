using Microsoft.EntityFrameworkCore;
using MovieCRUD_NCapas.DBContext;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;

namespace MovieCRUD_NCapas.Repository
{
    public class ActorRepository : GenericRepository<Actor>, IActorRepository
    {
        private readonly DBMovieContext _dbContext;

        public ActorRepository(DBMovieContext dbContext) :base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Actor>> GetActorsByIds(List<int> actorIds)
        {
            return await _dbContext.Actors.Where(a => actorIds.Contains(a.Id)).ToListAsync();
        }
    }
}
