namespace movie_ef_mvc.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int PeliculaId { get; set; }
        public Pelicula? Pelicula { get; set; }
        public string UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public string Comentario { get; set; }
        public int Raiting { get; set; }
        public DateTime FechaReview { get; set; }

    }
}
