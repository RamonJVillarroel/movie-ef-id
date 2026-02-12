using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace movie_ef_mvc.Models
{
    public class Pelicula
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        [Range(1,600)]
        public int MinutosDuracion { get; set; }
        [Required]
        [StringLength(1000)]
        public string Sinopsis { get; set; }
        [Required]
        [Url]
        public string PosterUrlPortada { get; set; }
        [NotMapped]
        public int PromedioRaiting { get; set; }
        public int PlataformaId { get; set; }
        public Plataforma? Plataforma { get; set; }
        public int GeneroId { get; set; }
        public Genero? Genero { get; set; }
        public List<Review>? ListaReviews { get; set; }
        public List<Favorito>? UsuariosFavorito { get; set; }

    }
}
