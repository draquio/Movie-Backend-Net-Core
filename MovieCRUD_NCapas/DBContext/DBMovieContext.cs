using Microsoft.EntityFrameworkCore;
using MovieCRUD_NCapas.Models;

namespace MovieCRUD_NCapas.DBContext
{
    public class DBMovieContext : DbContext
    {
        public DBMovieContext(DbContextOptions<DBMovieContext> options) : base(options)
        {
        }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<MovieActor> MoviesActors { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<MovieCategory> MovieCategories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Review>()
                .Navigation(review => review.Movie)
                .AutoInclude();
        }
    }
}
