namespace MovieCRUD_NCapas.DTO.Movie
{
    public class CreateMovieDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ReleaseDate { get; set; }
        public string? Duration { get; set; }
        public string? Poster { get; set; }
        public List<int>? ActorsIds { get; set; }
        public List<int>? CategoriesIds { get; set; }
    }
}
