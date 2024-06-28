
using AutoMapper;
using Moq;
using MovieCRUD_NCapas.DTO.Movie;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;
using MovieCRUD_NCapas.Services;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting_Service.Services.MovieTest
{
    public class MovieService_GetTests
    {
        private readonly Mock<IMovieRepository> _mockMovieRepository;
        private readonly IMapper _mapper;
        private readonly MovieService _movieService;
        private readonly MapperFunctions _mapperFunctions;
        int page = 1, pageSize = 10;
        List<Movie> movies = new List<Movie>
            {
                new Movie
                {
                    Id = 1,
                    Title = "Movie 1",
                    ReleaseDate = new DateTime(2020, 1, 1),
                    Duration = 120,
                    MovieCategories = new List<MovieCategory>(),
                    MovieActors = new List<MovieActor>()
                },
                new Movie
                {
                    Id = 2,
                    Title = "Movie 2",
                    ReleaseDate = new DateTime(2021, 2, 2),
                    Duration = 90,
                    MovieCategories = new List<MovieCategory>(),
                    MovieActors = new List<MovieActor>()
                }
            };
        public MovieService_GetTests()
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
        public async Task GetAll_ReturnsListOfMovieDTOs()
        {
            _mockMovieRepository.Setup(repo => repo.GetAll(page, pageSize))
                .ReturnsAsync(movies);
            var result = await _movieService.GetMovies(page, pageSize);

            Assert.Equal(2, result.Count);
            Assert.Equal("Movie 1", result[0].Title);
            Assert.Equal("01/01/2020", result[0].ReleaseDate);
            Assert.Equal("Movie 2", result[1].Title);
            Assert.Equal("02/02/2021", result[1].ReleaseDate);
        }
        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoMoviesExist()
        {
            _mockMovieRepository.Setup(repo => repo.GetAll(page, pageSize))
                .ReturnsAsync(new List<Movie>());
            var result = await _movieService.GetMovies(page, pageSize);
            Assert.Empty(result);
        }
        [Fact]
        public async Task GetAll_ThrowsException_WhenErrorOccurs()
        {
            _mockMovieRepository.Setup(repo => repo.GetAll(page, pageSize))
                .ThrowsAsync(new Exception("Test exception"));
            await Assert.ThrowsAsync<ApplicationException>(() => _movieService.GetMovies(page, pageSize));
        }
    }
}
