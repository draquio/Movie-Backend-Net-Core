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
        
        public new async Task<List<Movie>> GetAll()
        {
            try
            {
                List<Movie> movies = await _dbContext.Set<Movie>()
                    .Include(m => m.MovieCategories).ThenInclude(mc => mc.Category)
                    .Include(a => a.MovieActors).ThenInclude(ma => ma.Actor)
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
    }
}
