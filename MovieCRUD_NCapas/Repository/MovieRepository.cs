using Microsoft.EntityFrameworkCore;
using MovieCRUD_NCapas.DBContext;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;
using System.Collections.Generic;

namespace MovieCRUD_NCapas.Repository
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        private readonly DBMovieContext _dbContext;

        public MovieRepository(DBMovieContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
        
        public new async Task<List<Movie>> GetAllMovies(int page, int pageSize)
        {
            try
            {
                List<Movie> movies = await _dbContext.Set<Movie>()
                    .Include(m => m.MovieCategories).ThenInclude(mc => mc.Category)
                    .Include(a => a.MovieActors).ThenInclude(ma => ma.Actor)
                    .Skip((page - 1) * pageSize) // Omite los elementos de las páginas anteriores
                    .Take(pageSize) // Toma los elementos para la página actual
                    .ToListAsync();
                return movies;
            }
            catch
            {
                throw;
            }
        }

        public new async Task<Movie> GetById(int id)
        {
            try
            {
                Movie? movie = await _dbContext.Set<Movie>()
                    .Include(m => m.MovieCategories).ThenInclude(mc => mc.Category)
                    .Include(a => a.MovieActors).ThenInclude(ma => ma.Actor)
                    .FirstOrDefaultAsync(m => m.Id == id);
                return movie;
            }
            catch
            {
                throw;
            }
        }
        public new async Task<Movie> Create(Movie movie)
        {
            try
            {
                _dbContext.Set<Movie>().Add(movie);
                await _dbContext.SaveChangesAsync();
                return movie;
            }
            catch
            {
                throw;
            }
        }
        public new async Task<bool> Update(Movie movie)
        {
            try
            {
                _dbContext.Set<Movie>().Update(movie);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public new async Task<bool> Delete(Movie movie)
        {
            try
            {
                _dbContext.Set<Movie>().Remove(movie);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Movie> CreateMovie(Movie movie, List<int> actorsIds, List<int> categoriesIds)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await Create(movie);
                if (actorsIds != null && actorsIds.Any())
                {
                    foreach (int actorId in actorsIds)
                    {
                        MovieActor movieactor = new MovieActor
                        {
                            ActorId = actorId,
                            MovieId = movie.Id
                        };
                        _dbContext.MoviesActors.Add(movieactor);
                    }
                }
                if (categoriesIds != null && categoriesIds.Any())
                {
                    foreach (int categoryId in categoriesIds)
                    {
                        MovieCategory moviecategory = new MovieCategory
                        {
                            CategoryId = categoryId,
                            MovieId = movie.Id
                        };
                        _dbContext.MovieCategories.Add(moviecategory);
                    }  
                }
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                Movie? newMovie = await _dbContext.Set<Movie>()
                    .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
                    .Include(m => m.MovieCategories).ThenInclude(mc => mc.Category)
                    .FirstOrDefaultAsync(m => m.Id == movie.Id);
                return newMovie;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
