namespace MovieCRUD_NCapas.DTO
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
        public List<CategoryDTO>? Categories { get; set; }
        public List<ActorDTO>? Actors { get; set; }
    }
}
