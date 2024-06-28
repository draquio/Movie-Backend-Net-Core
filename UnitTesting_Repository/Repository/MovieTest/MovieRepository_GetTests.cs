
using Moq;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;

namespace UnitTesting_Repository.Repository.MovieTest
{
    public class MovieRepository_GetTests
    {
        private readonly Mock<IMovieRepository> _mockMovieRepository;
        private readonly IMovieRepository _movieRepository;
        int page = 1, pageSize = 10;
        List<Movie> movies = new List<Movie>
            {
                new Movie { Id = 1, Title = "Movie 1", Duration = 120 },
                new Movie { Id = 2, Title = "Movie 2", Duration = 90 }
            };
        public MovieRepository_GetTests()
        {
            _mockMovieRepository = new Mock<IMovieRepository>();
            _movieRepository = _mockMovieRepository.Object;
        }
        [Fact]
        public async Task Get_ReturnsListOfItems_WhenItemsExist()
        {
            _mockMovieRepository.Setup(repo => repo.GetAll(page, pageSize))
                .ReturnsAsync(movies);
            var result = await _movieRepository.GetAll(page, pageSize);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, m => m.Id == 1 && m.Title == "Movie 1");
            Assert.Contains(result, m => m.Id == 2 && m.Title == "Movie 2");
        }

        [Fact]
        public async Task Get_ReturnsEmptyList_WhenNoItemsExist()
        {

            _mockMovieRepository.Setup(repo => repo.GetAll(page, pageSize))
                .ReturnsAsync(new List<Movie>());
            var result = await _movieRepository.GetAll(page, pageSize);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAll_ThrowsException_WhenErrorOccurs()
        {
            _mockMovieRepository.Setup(repo => repo.GetAll(page, pageSize))
                .ThrowsAsync(new Exception("Test exception"));

            var exception = await Assert.ThrowsAsync<Exception>(() => _movieRepository.GetAll(page, pageSize));
            Assert.Equal("Test exception", exception.Message);
        }
    }
}
