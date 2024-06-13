using MovieCRUD_NCapas.Models;

namespace MovieCRUD_NCapas.Repository.Interface
{
    public interface IActorRepository : IGenericRepository<Actor>
    {
        Task<List<Actor>> GetActorsByIds(List<int> actorIds);
    }
}
