using MovieCRUD_NCapas.DTO;

namespace MovieCRUD_NCapas.Services.Interface
{
    public interface IActorService
    {
        Task<List<ActorDTO>> GetActors();
        Task<ActorDTO> GetById(int id);
        Task<ActorDTO> Create(ActorDTO actorDTO);
        Task<bool> Update(ActorDTO actorDTO);
        Task<bool> Delete(int id);
    }
}
