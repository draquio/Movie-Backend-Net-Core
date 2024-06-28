
using Moq;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository;
using MovieCRUD_NCapas.Repository.Interface;


namespace UnitTesting_Repository.Repository.MovieTest
{
    public class MovieRepository_CreateTests
    {
        private readonly Mock<IMovieRepository> _mockMovieRepository;
        private readonly IMovieRepository _movieRepository;
        Movie movie = new Movie { Id = 1, Title = "New Movie" };
        List<int> actorsIds = new List<int> { 1, 2 };
        List<int> categoriesIds = new List<int> { 1, 2 };
        public MovieRepository_CreateTests()
        {
            _mockMovieRepository = new Mock<IMovieRepository>();
            _movieRepository = _mockMovieRepository.Object;
        }
        [Fact]
        public async Task Create_ReturnsMovieWithActorsAndCategories_WhenMovieIsValid()
        {
            _mockMovieRepository.Setup(repo => repo.Create(movie, actorsIds, categoriesIds))
                .ReturnsAsync(movie);
            var result = await _movieRepository.Create(movie, actorsIds, categoriesIds);

            Assert.NotNull(result);
            Assert.Equal(movie.Id, result.Id);
            _mockMovieRepository.Verify(repo => repo.Create(movie, actorsIds, categoriesIds), Times.Once);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenErrorOccurs()
        {
            _mockMovieRepository.Setup(repo => repo.Create(movie, actorsIds, categoriesIds))
                .ThrowsAsync(new Exception("Test exception"));

            var exception = await Assert.ThrowsAsync<Exception>(() => _movieRepository.Create(movie, actorsIds, categoriesIds));
            Assert.Equal("Test exception", exception.Message);
        }
    }
}
