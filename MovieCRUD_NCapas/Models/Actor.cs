using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieCRUD_NCapas.Models
{
    public partial class Actor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsActive { get; set; }
        public string? Photo {  get; set; }
        public virtual ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();

    }
}
