using Microsoft.EntityFrameworkCore;
using MovieCRUD_NCapas.DBContext;
using MovieCRUD_NCapas.Repository.Interface;
using System.Linq.Expressions;

namespace MovieCRUD_NCapas.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DBMovieContext _dbContext;

        public GenericRepository(DBMovieContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<T>> GetAll(int page, int pageSize)
        {
            try
            {
                List<T> model = await _dbContext.Set<T>()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                return model;
            }
            catch
            {
                throw;
            }
        }

        public async Task<T> GetById(int id)
        {
            try
            {
                T model = await _dbContext.Set<T>().FindAsync(id);
                return model;
            }
            catch
            {
                throw;
            }
        }
        public async Task<T> Create(T model)
        {
            try
            {
                _dbContext.Set<T>().Add(model);
                await _dbContext.SaveChangesAsync();
                return model;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Update(T model)
        {
            try
            {
                _dbContext.Set<T>().Update(model);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Delete(T model)
        {
            try
            {
                _dbContext.Set<T>().Remove(model);
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
