using MovieCRUD_NCapas.DTO.Actor;

namespace MovieCRUD_NCapas.Services.Interface
{
    public interface IActorService
    {
        Task<List<ActorDTO>> GetActors(int page, int pageSize);
        Task<ActorDTO> GetById(int id);
        Task<ActorDTO> Create(ActorDTO actorDTO);
        Task<bool> Update(ActorDTO actorDTO);
        Task<bool> Delete(int id);
    }
}
