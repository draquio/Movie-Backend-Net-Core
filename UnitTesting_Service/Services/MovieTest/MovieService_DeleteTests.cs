
using Moq;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;
using MovieCRUD_NCapas.Services;
using MovieCRUD_NCapas.Utility;

namespace UnitTesting_Service.Services.MovieTest
{
    public class MovieService_DeleteTests
    {
        private readonly Mock<IMovieRepository> _mockMovieRepository;
        private readonly MovieService _movieService;
        private readonly MapperFunctions _mapperFunctions;
        int movieId = 1;
        Movie existingMovie = new Movie { Id = 1, Title = "Existing Movie" };
        public MovieService_DeleteTests()
        {
            _mockMovieRepository = new Mock<IMovieRepository>();
            _movieService = new MovieService(_mockMovieRepository.Object, null);
        }
        [Fact]
        public async Task Delete_ReturnsTrue_WhenDeleteIsSuccessful()
        {
            _mockMovieRepository.Setup(repo => repo.GetById(movieId))
                .ReturnsAsync(existingMovie);
            _mockMovieRepository.Setup(repo => repo.Delete(existingMovie))
                .ReturnsAsync(true);
            var result = await _movieService.Delete(movieId);

            Assert.True(result);
        }
        [Fact]
        public async Task Delete_ThrowsApplicationException_WhenMovieNotFound()
        {
            _mockMovieRepository.Setup(repo => repo.GetById(movieId))
                .ReturnsAsync((Movie)null);

            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _movieService.Delete(movieId));
            Assert.Contains($"An error occurred while deleting the Movie: Movie with ID {movieId} not found", exception.Message);
        }
        [Fact]
        public async Task Delete_ThrowsApplicationException_WhenDeleteFails()
        {
            _mockMovieRepository.Setup(repo => repo.GetById(movieId))
                .ReturnsAsync(existingMovie);
            _mockMovieRepository.Setup(repo => repo.Delete(existingMovie))
                .ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _movieService.Delete(movieId));
            Assert.Contains("An error occurred while deleting the Movie: Movie couldn't be deleted", exception.Message);
        }
        [Fact]
        public async Task Delete_ThrowsApplicationException_WhenErrorOccurs()
        {
            _mockMovieRepository.Setup(repo => repo.GetById(movieId))
                .ReturnsAsync(existingMovie);
            _mockMovieRepository.Setup(repo => repo.Delete(existingMovie))
                .ThrowsAsync(new Exception("Test exception"));

            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _movieService.Delete(movieId));
            Assert.Contains("An error occurred while deleting the Movie: Test exception", exception.Message);
        }
    }
}
