using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieCRUD_NCapas.Models
{
    public partial class MovieActor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int ActorId { get; set; }
        public virtual Movie? Movie { get; set; }
        public virtual Actor? Actor { get; set; }
    }
}
