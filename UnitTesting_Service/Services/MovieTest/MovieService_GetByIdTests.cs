

using AutoMapper;
using Moq;
using MovieCRUD_NCapas.DTO.Movie;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;
using MovieCRUD_NCapas.Services;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting_Service.Services.MovieTest
{
    public class MovieService_GetByIdTests
    {
        private readonly Mock<IMovieRepository> _mockMovieRepository;
        private readonly IMapper _mapper;
        private readonly MovieService _movieService;
        private readonly MapperFunctions _mapperFunctions;
        int movieId = 1;
        Movie movie = new Movie
        {
            Id = 1,
            Title = "Movie 1",
            ReleaseDate = new DateTime(2021, 2, 2),
            Duration = 90,
            MovieCategories = new List<MovieCategory>(),
            MovieActors = new List<MovieActor>()
        };

        public MovieService_GetByIdTests()
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
            });
            _mapper = config.CreateMapper();
            _movieService = new MovieService(_mockMovieRepository.Object, _mapper);
        }
        [Fact]
        public async Task GetById_ReturnsItem_WhenItemExists()
        {
            _mockMovieRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(movie);
            var result = await _movieService.GetById(movieId);
            Assert.NotNull(result);
            Assert.Equal(movie.Id, result.Id);
            Assert.Equal("Movie 1", result.Title);
        }
        [Fact]
        public async Task GetById_ReturnsNull_WhenItemDoesNotExist()
        {
            _mockMovieRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync((Movie)null);

            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _movieService.GetById(movieId));
            Assert.Contains($"An error occurred while retrieving the Movie: Movie with ID {movieId} not found", exception.Message);
        }
        [Fact]
        public async Task GetById_ThrowsException_WhenErrorOccurs()
        {
            _mockMovieRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));
            await Assert.ThrowsAsync<ApplicationException>(() => _movieService.GetById(movieId));
        }
    }
}
