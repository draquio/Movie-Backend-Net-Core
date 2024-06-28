

using Moq;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;

namespace UnitTesting_Repository.Repository.MovieTest
{
    public class MovieRepository_DeleteTests
    {
        private readonly Mock<IMovieRepository> _mockMovieRepository;
        private readonly IMovieRepository _movieRepository;
        Movie movie = new Movie { Id = 1, Title = "Movie" };
        public MovieRepository_DeleteTests()
        {
            _mockMovieRepository = new Mock<IMovieRepository>();
            _movieRepository = _mockMovieRepository.Object;
        }
        [Fact]
        public async Task Delete_ReturnsTrue_WhenDeleteIsSuccessful()
        {
            _mockMovieRepository.Setup(repo => repo.Delete(movie))
                .ReturnsAsync(true);
            var result = await _movieRepository.Delete(movie);

            Assert.True(result);
        }

        [Fact]
        public async Task Delete_ReturnsFalse_WhenDeleteFails()
        {
            _mockMovieRepository.Setup(repo => repo.Delete(movie)).ReturnsAsync(false);
            var result = await _movieRepository.Delete(movie);

            Assert.False(result);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenErrorOccurs()
        {
            _mockMovieRepository.Setup(repo => repo.Delete(movie))
                .ThrowsAsync(new Exception("Test exception"));

            var exception = await Assert.ThrowsAsync<Exception>(() => _movieRepository.Delete(movie));

            Assert.Equal("Test exception", exception.Message);
        }
    }
}
