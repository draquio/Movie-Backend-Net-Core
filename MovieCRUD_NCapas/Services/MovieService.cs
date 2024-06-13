using AutoMapper;
using MovieCRUD_NCapas.DTO.Movie;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository;
using MovieCRUD_NCapas.Repository.Interface;
using MovieCRUD_NCapas.Services.Interface;

namespace MovieCRUD_NCapas.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MovieService(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<List<MovieDTO>> GetMovies(int page, int pageSize)
        {
            try
            {
                List<Movie> listMovie = await _movieRepository.GetAllMovies(page, pageSize);
                if (listMovie == null)
                {
                    return new List<MovieDTO>();
                }
                List<MovieDTO> listMovieDTO = _mapper.Map<List<MovieDTO>>(listMovie);
                return listMovieDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving the Movies: {ex.Message}", ex);
            }
        }
        public async Task<MovieDTO> GetById(int id)
        {
            try
            {
                Movie movieSearched = await _movieRepository.GetById(id);
                if (movieSearched == null)
                {
                    throw new KeyNotFoundException($"Movie with ID {id} not found");
                }
                MovieDTO movieDTO = _mapper.Map<MovieDTO>(movieSearched);
                return movieDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving the Movie: {ex.Message}", ex);
            }
        }
        public async Task<MovieDTO> Create(CreateMovieDTO createmovieDTO)
        {
            try
            {
                Movie movie = _mapper.Map<Movie>(createmovieDTO);
                Movie movieCreated = await _movieRepository.CreateMovie(movie, createmovieDTO.ActorsIds, createmovieDTO.CategoriesIds);
                if (movieCreated == null || movieCreated.Id == 0)
                {
                    throw new InvalidOperationException("Movie couldn't be created");
                }
                MovieDTO movieCreatedDTO = _mapper.Map<MovieDTO>(movieCreated);
                return movieCreatedDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while creating the Movie: {ex.Message}", ex);
            }
        }
        public async Task<bool> Update(MovieDTO movieDTO)
        {
            try
            {
                Movie movieSearched = await _movieRepository.GetById(movieDTO.Id);
                if (movieSearched == null)
                {
                    throw new KeyNotFoundException($"Movie with ID {movieDTO.Id} not found");
                }
                _mapper.Map(movieDTO, movieSearched);
                bool response = await _movieRepository.Update(movieSearched);
                if (!response)
                {
                    throw new InvalidOperationException("Movie couldn't be updated");
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while editing the Movie: {ex.Message}", ex);
            }
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                Movie movieSearched = await _movieRepository.GetById(id);
                if (movieSearched == null)
                {
                    throw new KeyNotFoundException($"Movie with ID {id} not found");
                }
                bool response = await _movieRepository.Delete(movieSearched);
                if (!response)
                {
                    throw new InvalidOperationException("Movie couldn't be deleted");
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting the Movie: {ex.Message}", ex);
            }
        }






    }
}
