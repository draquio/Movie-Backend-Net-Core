using Moq;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;
namespace UnitTesting_Repository.Repository.MovieTest
{
    public class MovieRepository_GetByIdTests
    {
        private readonly Mock<IMovieRepository> _mockMovieRepository;
        private readonly IMovieRepository _movieRepository;
        Movie movie = new Movie { Id = 1, Title = "Movie 1" };
        int movieId = 1;
        public MovieRepository_GetByIdTests()
        {
            _mockMovieRepository = new Mock<IMovieRepository>();
            _movieRepository = _mockMovieRepository.Object;
        }
        [Fact]
        public async Task GetById_ReturnsItem_WhenItemExists()
        {

            _mockMovieRepository.Setup(repo => repo.GetById(movieId))
                .ReturnsAsync(movie);
            var result = await _movieRepository.GetById(movieId);

            Assert.NotNull(result);
            Assert.Equal(movie.Id, result.Id);
            Assert.Equal(movie.Title, result.Title);
        }

        [Fact]
        public async Task GetById_ReturnsNull_WhenMovieDoesNotExist()
        {
            int movieId = 2;
            _mockMovieRepository.Setup(repo => repo.GetById(movieId))
                .ReturnsAsync((Movie)null);
            var result = await _movieRepository.GetById(movieId);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetById_ThrowsException_WhenErrorOccurs()
        {
            _mockMovieRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));

            var exception = await Assert.ThrowsAsync<Exception>(() => _movieRepository.GetById(movieId));
            Assert.Equal("Test exception", exception.Message);
        }
    }
}
