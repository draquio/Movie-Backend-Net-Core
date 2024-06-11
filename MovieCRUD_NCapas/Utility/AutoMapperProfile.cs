using AutoMapper;
using MovieCRUD_NCapas.DTO;
using MovieCRUD_NCapas.Models;
using System.Globalization;

namespace MovieCRUD_NCapas.Utility
{
    public class AutoMapperProfile : Profile
    {
        //private readonly MapperFunctions _mapperFunctions;
        // public AutoMapperProfile(MapperFunctions mapperFuntions) {
        public AutoMapperProfile() {
            //_mapperFunctions = mapperFuntions;

            #region Category
            CreateMap<Category, CategoryDTO>()
                .ForMember(dto => dto.IsActive, options => options.MapFrom(category => category.IsActive == true ? 1 : 0));

            CreateMap<CategoryDTO, Category>()
                .ForMember(category => category.IsActive, options => options.MapFrom(dto => dto.IsActive == 1 ? true : false));
            #endregion

            #region Actor
            CreateMap<Actor, ActorDTO>()
                .ForMember(dto => dto.BirthDate, options => options.MapFrom(actor => actor.BirthDate.ToString("dd/MM/yyyy")))
                .ForMember(dto => dto.IsActive, options => options.MapFrom(actor => actor.IsActive == true ? 1 : 0));
            CreateMap<ActorDTO, Actor>()
                .ForMember(actor => actor.BirthDate, options => options.MapFrom(dto => DateTime.ParseExact(dto.BirthDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(actor => actor.IsActive, options => options.MapFrom(dto => dto.IsActive == 1 ? true : false));

            #endregion

            #region Review
            CreateMap<Review, ReviewDTO>()
                .ForMember(dto => dto.MovieId, options => options.MapFrom(review => review.MovieId))
                .ForMember(dto => dto.MovieName, options => options.MapFrom(review => review.Movie.Title))
                .ForMember(dto => dto.Rating, options => options.MapFrom(review => $"{review.Rating}/5"))
                .ForMember(dto => dto.ReviewDate, options => options.MapFrom(review => review.ReviewDate.ToString("dd/MM/yyyy")));

            CreateMap<ReviewDTO, Review>()
                .ForMember(review => review.Rating, options => options.MapFrom(dto => int.Parse(dto.Rating)))
                .ForMember(review => review.ReviewDate, options => options.MapFrom(dto => DateTime.ParseExact(dto.ReviewDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)));

            #endregion
            /*
            #region Movie
            CreateMap<Movie, MovieDTO>()
                .ForMember(dto => dto.ReleaseDate, options => options.MapFrom(movie => movie.ReleaseDate.ToString("dd/MM/yyyy")))
                .ForMember(dto => dto.Duration, options => options.MapFrom(movie => _mapperFunctions.FormatDuration(movie.Duration)))
                .ForMember(dto => dto.Rating, options => options.MapFrom(movie => _mapperFunctions.CalculateRating(movie.Reviews)))
                .ForMember(dto => dto.Categories, options => options.MapFrom(movie => movie.MovieCategories))
                .ForMember(dto => dto.Actors, options => options.MapFrom(movie => movie.MovieActors));
            CreateMap<MovieDTO, Movie>()
                .ForMember(movie => movie.ReleaseDate, options => options.MapFrom(dto => _mapperFunctions.DateTimeFormat(dto.ReleaseDate)))
                .ForMember(movie => movie.Duration, options => options.MapFrom(dto => int.Parse(dto.Duration)));
            #endregion
            */
        }

    }
}
