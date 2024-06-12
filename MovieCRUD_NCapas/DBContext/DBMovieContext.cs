﻿using Microsoft.EntityFrameworkCore;
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

            //Auto incluir la pelicula en la review
            modelBuilder.Entity<Review>()
                .Navigation(review => review.Movie)
                .AutoInclude();

            // Movie Category -> muchos a muchos
            modelBuilder.Entity<MovieCategory>()
                .HasOne(mc => mc.Movie)
                .WithMany(m => m.MovieCategories)
                .HasForeignKey(mc => mc.MovieId);
            modelBuilder.Entity<MovieCategory>()
                .HasOne(mc => mc.Category)
                .WithMany(c => c.MovieCategories)
                .HasForeignKey(mc => mc.CategoryId);


            // Movie Actor -> muchos a muchos
            modelBuilder.Entity<MovieActor>()
                .HasOne(mc => mc.Movie)
                .WithMany(a => a.MovieActors)
                .HasForeignKey(mc => mc.MovieId);
            modelBuilder.Entity<MovieActor>()
                .HasOne(mc => mc.Actor)
                .WithMany(a => a.MovieActors)
                .HasForeignKey(mc => mc.ActorId);
        }
    }
}
