namespace MovieCRUD_NCapas.DTO
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public string? MovieName { get; set; }
        public int MovieId { get; set; }
        public string? ReviewerName { get; set; }
        public string? Rating { get; set; }
        public string? Comment { get; set; }
        public string? ReviewDate { get; set; }


    }
}
