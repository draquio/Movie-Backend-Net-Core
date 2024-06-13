using MovieCRUD_NCapas.DTO.Actor;
using MovieCRUD_NCapas.DTO.Category;
using MovieCRUD_NCapas.Models;

namespace MovieCRUD_NCapas.DTO.Movie
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ReleaseDate { get; set; }
        public string? Duration { get; set; }
        public string? Rating { get; set; }
        public string? Poster { get; set; }
        public List<CategoryResponseDTO>? Categories { get; set; }
        public List<ActorResponseDTO>? Actors { get; set; }
    }
}
