using OllamaSharp;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace movie_ef_mvc.Services
{
    public class OllamaLlmServices // <--- Nombre corregido
    {
        private readonly IOllamaApiClient _ollamaClient;

        public OllamaLlmServices(IOllamaApiClient ollamaClient)
        {
            _ollamaClient = ollamaClient;
        }

        public async Task<string> ObtenerSpoilerAsync(string tituloPelicula)
        {
            if (string.IsNullOrWhiteSpace(tituloPelicula))
                throw new ArgumentException("El título no puede estar vacío.", nameof(tituloPelicula));

            string prompt = $@"Genera un pequeño spoiler (máximo 2-3 oraciones) sobre la película ""{tituloPelicula}"". 
                               Revela un giro interesante de la trama de forma concisa.";

            return await ConsultarLlmAsync(prompt);
        }

        public async Task<string> ObtenerResumenAsync(string tituloPelicula)
        {
            if (string.IsNullOrWhiteSpace(tituloPelicula))
                throw new ArgumentException("El título no puede estar vacío.", nameof(tituloPelicula));

            string prompt = $@"Proporciona un resumen breve (3-4 oraciones) de la película ""{tituloPelicula}"". 
                               Incluye género y premisa principal sin spoilers.";

            return await ConsultarLlmAsync(prompt);
        }

        public async Task<string> ConsultaSimpleAsync(string pregunta)
        {
            if (string.IsNullOrWhiteSpace(pregunta))
                throw new ArgumentException("La pregunta no puede estar vacía.", nameof(pregunta));

            return await ConsultarLlmAsync(pregunta);
        }

        private async Task<string> ConsultarLlmAsync(string prompt)
        {
            try
            {
                var sb = new StringBuilder();

                // 1. Iniciamos el flujo
                var flujo = _ollamaClient.GenerateAsync(prompt);

                // 2. Recorremos el flujo manualmente (esto evita el error de Select)
                await foreach (var respuesta in flujo)
                {
                    if (respuesta != null && !string.IsNullOrEmpty(respuesta.Response))
                    {
                        sb.Append(respuesta.Response);
                    }
                }

                return sb.ToString().Trim() ?? "No se pudo obtener respuesta.";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error con Ollama local: {ex.Message}", ex);
            }
        }
    }
}