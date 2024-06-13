using AutoMapper;
using MovieCRUD_NCapas.DTO.Actor;
using MovieCRUD_NCapas.DTO.Category;
using MovieCRUD_NCapas.DTO.Movie;
using MovieCRUD_NCapas.DTO.Review;
using MovieCRUD_NCapas.Models;
using System.Globalization;

namespace MovieCRUD_NCapas.Utility
{
    public class AutoMapperProfile : Profile
    {
        private readonly MapperFunctions _mapperFunctions;
        public AutoMapperProfile(MapperFunctions mapperFunctions) {
            _mapperFunctions = mapperFunctions;



            #region Category
            CreateMap<Category, CategoryDTO>()
                .ForMember(dto => dto.IsActive, options => options.MapFrom(category => category.IsActive == true ? 1 : 0));

            CreateMap<CategoryDTO, Category>()
                .ForMember(category => category.IsActive, options => options.MapFrom(dto => dto.IsActive == 1 ? true : false));

            CreateMap<Category, CategoryResponseDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            #endregion

            #region Actor
            CreateMap<Actor, ActorDTO>()
                .ForMember(dto => dto.BirthDate, options => options.MapFrom(actor => actor.BirthDate.ToString("dd/MM/yyyy")))
                .ForMember(dto => dto.IsActive, options => options.MapFrom(actor => actor.IsActive == true ? 1 : 0));
            CreateMap<ActorDTO, Actor>()
                .ForMember(actor => actor.BirthDate, options => options.MapFrom(dto => DateTime.ParseExact(dto.BirthDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(actor => actor.IsActive, options => options.MapFrom(dto => dto.IsActive == 1 ? true : false));

            CreateMap<Actor, ActorResponseDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
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
            #region Movie
            CreateMap<Movie, MovieDTO>()
                .ForMember(dto => dto.ReleaseDate, options => options.MapFrom(movie => movie.ReleaseDate.ToString("dd/MM/yyyy")))
                .ForMember(dto => dto.Duration, options => options.MapFrom(movie => _mapperFunctions.FormatDuration(movie.Duration)))
                .ForMember(dto => dto.Rating, options => options.MapFrom(movie => "No data"))
                .ForMember(dto => dto.Categories, options => options.MapFrom(movie => movie.MovieCategories.Select(mc => mc.Category)))
                .ForMember(dto => dto.Actors, options => options.MapFrom(movie => movie.MovieActors.Select(ma => ma.Actor)));

            CreateMap<MovieDTO, Movie>()
                .ForMember(movie => movie.ReleaseDate, options => options.MapFrom(dto => _mapperFunctions.DateTimeFormat(dto.ReleaseDate)))
                .ForMember(movie => movie.Duration, options => options.MapFrom(dto => int.Parse(dto.Duration)));

            CreateMap<CreateMovieDTO, Movie>()
                .ForMember(movie => movie.ReleaseDate, options => options.MapFrom(dto => _mapperFunctions.DateTimeFormat(dto.ReleaseDate)))
                .ForMember(movie => movie.Duration, options => options.MapFrom(dto => int.Parse(dto.Duration)))
                .ForMember(dest => dest.MovieActors, opt => opt.Ignore())
                .ForMember(dest => dest.MovieCategories, opt => opt.Ignore());
            #endregion

        }

    }
}
