

using AutoMapper;
using Moq;
using MovieCRUD_NCapas.DTO.Movie;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;
using MovieCRUD_NCapas.Services;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting_Service.Services.MovieTest
{
    public class MovieService_UpdateTests
    {
        private readonly Mock<IMovieRepository> _mockMovieRepository;
        private readonly IMapper _mapper;
        private readonly MovieService _movieService;
        private readonly MapperFunctions _mapperFunctions;
        MovieDTO movieDTO = new MovieDTO
        {
            Id = 1,
            Title = "Updated Movie",
            ReleaseDate = "01/01/2023",
            Duration = "150",
        };
        Movie existingMovie = new Movie
        {
            Id = 1,
            Title = "Existing Movie",
            ReleaseDate = new DateTime(2023, 1, 1),
            Duration = 120
        };
        Movie updatedMovie = new Movie
        {
            Id = 1,
            Title = "Updated Movie",
            ReleaseDate = new DateTime(2023, 1, 1),
            Duration = 150
        };

        public MovieService_UpdateTests()
        {
            _mockMovieRepository = new Mock<IMovieRepository>();
            _mapperFunctions = new MapperFunctions();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Movie, MovieDTO>()
                    .ForMember(dto => dto.ReleaseDate, options => options.MapFrom(movie => movie.ReleaseDate.ToString("dd/MM/yyyy")))
                    .ForMember(dto => dto.Duration, options => options.MapFrom(movie => _mapperFunctions.FormatDuration(movie.Duration)))
                    .ForMember(dto => dto.Rating, options => options.MapFrom(movie => "No data"))
                    .ForMember(dto => dto.Categories, options => options.MapFrom(movie => movie.MovieCategories.Select(mc => mc.Category)))
                    .ForMember(dto => dto.Actors, options => options.MapFrom(movie => movie.MovieActors.Select(ma => ma.Actor)));

                cfg.CreateMap<MovieDTO, Movie>()
                    .ForMember(movie => movie.ReleaseDate, options => options.MapFrom(dto => _mapperFunctions.DateTimeFormat(dto.ReleaseDate)))
                    .ForMember(movie => movie.Duration, options => options.MapFrom(dto => int.Parse(dto.Duration)));
                cfg.CreateMap<CreateMovieDTO, Movie>()
                    .ForMember(movie => movie.ReleaseDate, options => options.MapFrom(dto => _mapperFunctions.DateTimeFormat(dto.ReleaseDate)))
                    .ForMember(movie => movie.Duration, options => options.MapFrom(dto => int.Parse(dto.Duration)))
                    .ForMember(dest => dest.MovieActors, opt => opt.Ignore())
                    .ForMember(dest => dest.MovieCategories, opt => opt.Ignore());
            });
            _mapper = config.CreateMapper();
            _movieService = new MovieService(_mockMovieRepository.Object, _mapper);
        }
        [Fact]
        public async Task Update_ReturnsTrue_WhenUpdatedIsSuccessful()
        {
            _mockMovieRepository.Setup(repo => repo.GetById(existingMovie.Id))
                .ReturnsAsync(existingMovie);
            _mockMovieRepository.Setup(repo => repo.Update(existingMovie))
                .ReturnsAsync(true);

            var result = await _movieService.Update(movieDTO);
            Assert.True(result);
        }
        [Fact]
        public async Task Update_ReturnsFalse_WhennUpdateFails()
        {
            _mockMovieRepository.Setup(repo => repo.GetById(movieDTO.Id))
                .ReturnsAsync(existingMovie);
            _mockMovieRepository.Setup(repo => repo.Update(It.IsAny<Movie>()))
                .ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _movieService.Update(movieDTO));
            Assert.Contains("An error occurred while editing the Movie: Movie couldn't be updated", exception.Message);
        }
        [Fact]
        public async Task Update_ThrowsApplicationException_WhenItemNotFound()
        {
            _mockMovieRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync((Movie)null);
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _movieService.Update(movieDTO));
            Assert.Contains($"An error occurred while editing the Movie: Movie with ID {movieDTO.Id} not found", exception.Message);
        }

        [Fact]
        public async Task Update_ThrowsApplicationException_WhenErrorOccurs()
        {
            _mockMovieRepository.Setup(repo => repo.GetById(movieDTO.Id))
                .ReturnsAsync(existingMovie);
            _mockMovieRepository.Setup(repo => repo.Update(It.IsAny<Movie>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _movieService.Update(movieDTO));
            Assert.Contains("An error occurred while editing the Movie: Test exception", exception.Message);
        }
    }
}
