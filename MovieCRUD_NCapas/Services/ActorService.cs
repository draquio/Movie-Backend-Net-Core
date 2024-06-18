using AutoMapper;
using MovieCRUD_NCapas.DTO.Actor;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;
using MovieCRUD_NCapas.Services.Interface;

namespace MovieCRUD_NCapas.Services
{
    public class ActorService : IActorService
    {
        private readonly IGenericRepository<Actor> _actorRepository;
        private readonly IMapper _mapper;

        public ActorService(IGenericRepository<Actor> actorRepository, IMapper mapper)
        {
            _actorRepository = actorRepository;
            _mapper = mapper;
        }

        public async Task<List<ActorDTO>> GetActors(int page, int pageSize)
        {
            try
            {
                List<Actor> listActors = await _actorRepository.GetAll(page, pageSize);
                if (listActors == null || !listActors.Any())
                {
                    return new List<ActorDTO>();
                }
                List<ActorDTO> listactorsActorsDTO = _mapper.Map<List<ActorDTO>>(listActors);
                return listactorsActorsDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving the Actors: {ex.Message}", ex);
            }
        }

        public async Task<ActorDTO> GetById(int id)
        {
            try
            {
                Actor actorSearched = await _actorRepository.GetById(id);
                if (actorSearched == null)
                {
                    throw new KeyNotFoundException($"Actor with ID {id} not found");
                }
                ActorDTO actorDTO = _mapper.Map<ActorDTO>(actorSearched);
                return actorDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving the Actor: {ex.Message}", ex);
            }
        }
        public async Task<ActorDTO> Create(ActorDTO actorDTO)
        {
            try
            {
                Actor actor = _mapper.Map<Actor>(actorDTO);
                Actor actorCreated = await _actorRepository.Create(actor);
                if (actorCreated == null || actorCreated.Id == 0)
                {
                    throw new InvalidOperationException("Actor couldn't be created");
                }
                ActorDTO actorCreatedDTO = _mapper.Map<ActorDTO>(actorCreated);
                return actorCreatedDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while creating the Actor: {ex.Message}", ex);
            }
        }
        public async Task<bool> Update(ActorDTO actorDTO)
        {
            try
            {
                Actor actorSearched = await _actorRepository.GetById(actorDTO.Id);
                if (actorSearched == null)
                {
                    throw new KeyNotFoundException($"Actor with ID {actorDTO.Id} not found");
                }
                _mapper.Map(actorDTO, actorSearched);
                bool response = await _actorRepository.Update(actorSearched);
                if (!response)
                {
                    throw new InvalidOperationException("Actor couldn't be updated");
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while updating the Actor: {ex.Message}", ex);
            }
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                Actor actorSearched = await _actorRepository.GetById(id);
                if (actorSearched == null)
                {
                    throw new KeyNotFoundException($"Actor with ID {id} not found");
                }
                bool response = await _actorRepository.Delete(actorSearched);
                if (!response)
                {
                    throw new InvalidOperationException("Actor couldn't be deleted");
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting the Actor: {ex.Message}", ex);
            }
        }
    }
}
