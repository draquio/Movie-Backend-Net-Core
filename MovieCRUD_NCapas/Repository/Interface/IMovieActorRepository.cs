using MovieCRUD_NCapas.Models;

namespace MovieCRUD_NCapas.Repository.Interface
{
    public interface IMovieActorRepository
    {
        Task CreateMovieActor(MovieActor movieActor);
    }
}

