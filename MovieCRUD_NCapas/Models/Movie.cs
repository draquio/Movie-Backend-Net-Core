using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieCRUD_NCapas.Models
{
    public partial class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }
        public string? Poster {  get; set; }
        public virtual ICollection<MovieCategory> MovieCategories { get; set; } = new List<MovieCategory>();
        public virtual ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
