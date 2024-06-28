

using Moq;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;

namespace UnitTesting_Repository.Repository.MovieTest
{
    public class MovieRepository_UpdateTests
    {
        private readonly Mock<IMovieRepository> _mockMovieRepository;
        private readonly IMovieRepository _movieRepository;
        Movie movie = new Movie { Id = 1, Title = "Updated Movie" };
        public MovieRepository_UpdateTests()
        {
            _mockMovieRepository = new Mock<IMovieRepository>();
            _movieRepository = _mockMovieRepository.Object;
        }
        [Fact]
        public async Task Update_ReturnsTrue_WhenUpdateIsSuccessful()
        {
            _mockMovieRepository.Setup(repo => repo.Update(movie))
                .ReturnsAsync(true);
            var result = await _movieRepository.Update(movie);
            Assert.True(result);
        }

        [Fact]
        public async Task Update_ReturnsFalse_WhenUpdateFails()
        {
            _mockMovieRepository.Setup(repo => repo.Update(movie))
                .ReturnsAsync(false);
            var result = await _movieRepository.Update(movie);

            Assert.False(result);
        }

        [Fact]
        public async Task Update_ThrowsException_WhenErrorOccurs()
        {

            _mockMovieRepository.Setup(repo => repo.Update(movie))
                .ThrowsAsync(new Exception("Test exception"));
            var exception = await Assert.ThrowsAsync<Exception>(() => _movieRepository.Update(movie));
            Assert.Equal("Test exception", exception.Message);
        }
    }
}
