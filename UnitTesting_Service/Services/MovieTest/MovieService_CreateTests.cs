
using AutoMapper;
using Moq;
using MovieCRUD_NCapas.DTO.Movie;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;
using MovieCRUD_NCapas.Services;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting_Service.Services.MovieTest
{
    public class MovieService_CreateTests
    {
        private readonly Mock<IMovieRepository> _mockMovieRepository;
        private readonly IMapper _mapper;
        private readonly MovieService _movieService;
        private readonly MapperFunctions _mapperFunctions;

        CreateMovieDTO createMovieDTO = new CreateMovieDTO
        {
            Title = "New Movie",
            ReleaseDate = "01/01/2024",
            Duration = "120",
            ActorsIds = new List<int> { 1, 2 },
            CategoriesIds = new List<int> { 3, 4 }
        };
        Movie createdMovie = new Movie
        {
            Id = 1,
            Title = "New Movie",
            ReleaseDate = new DateTime(2023, 1, 1),
            Duration = 120,
            MovieCategories = new List<MovieCategory>(),
            MovieActors = new List<MovieActor>()
        };
        public MovieService_CreateTests()
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
        public async Task Create_ReturnsCreatedItem()
        {
            _mockMovieRepository.Setup(repo => repo.Create(It.IsAny<Movie>(), It.IsAny<List<int>>(), It.IsAny<List<int>>()))
                .ReturnsAsync(createdMovie);
            var result = await _movieService.Create(createMovieDTO);

            Assert.NotNull(result);
            Assert.Equal(createdMovie.Id, result.Id);
            Assert.Equal("New Movie", result.Title);
        }
        [Fact]
        public async Task Create_ThrowsApplicationException_WhenCreationFails()
        {

            _mockMovieRepository.Setup(repo => repo.Create(It.IsAny<Movie>(), It.IsAny<List<int>>(), It.IsAny<List<int>>()))
                .ReturnsAsync((Movie)null);
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _movieService.Create(createMovieDTO));
            Assert.Contains("An error occurred while creating the Movie: Movie couldn't be created", exception.Message);
        }
        [Fact]
        public async Task Create_ThrowsApplicationException_WhenErrorOccurs()
        {
            _mockMovieRepository.Setup(repo => repo.Create(It.IsAny<Movie>(), It.IsAny<List<int>>(), It.IsAny<List<int>>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _movieService.Create(createMovieDTO));
            Assert.Contains("An error occurred while creating the Movie: Test exception", exception.Message);
        }
    }
}
