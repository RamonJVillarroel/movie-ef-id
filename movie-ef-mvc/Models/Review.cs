using System.ComponentModel.DataAnnotations;

namespace movie_ef_mvc.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int PeliculaId { get; set; }
        public Pelicula? Pelicula { get; set; }
        public string UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        [Required]
        [StringLength(500)]
        public string Comentario { get; set; }
        [Range(1, 5)]
        public int Raiting { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaReview { get; set; }
        //este campo ayudara a que entity framework pueda controlar la concurrencia optimista, es decir, si dos usuarios intentan editar la misma review al mismo tiempo, el sistema podrá detectar el conflicto y evitar que se sobrescriban los cambios de uno de ellos.
        //row version para control de concurrencia optimista
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}
