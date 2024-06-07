using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieCRUD_NCapas.Models
{
    public partial class MovieCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? MovieId { get; set; }
        public int? CategoryId { get; set; }
        public virtual Movie? Movie { get; set; }
        public virtual Category? Category { get; set; }
    }
}
