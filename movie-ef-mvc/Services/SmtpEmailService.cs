using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace movie_ef_mvc.Services
{
    // Interfaz para el servicio de correo electrónico, define un método para enviar correos de forma asíncrona.
    public interface IEmailService
        {
            Task SendAsync(string to, string subject, string htmlBody, string? textBody = null);
        }
    // Implementación del servicio de correo electrónico utilizando SMTP. Utiliza la biblioteca MailKit para enviar correos electrónicos.
    public sealed class SmtpEmailService : IEmailService
        {
            private readonly SmtpSettings _cfg;
            public SmtpEmailService(IOptions<SmtpSettings> cfg) => _cfg = cfg.Value;
        // Método para enviar un correo electrónico de forma asíncrona.
        // Construye el mensaje utilizando MimeKit, se conecta al servidor SMTP, autentica y envía el mensaje.
        public async Task SendAsync(string to, string subject, string htmlBody, string? textBody = null)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_cfg.FromName, _cfg.User));
                message.To.Add(MailboxAddress.Parse(to));
                message.Subject = subject;

                var builder = new BodyBuilder { HtmlBody = htmlBody, TextBody = textBody ?? string.Empty };
                message.Body = builder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(_cfg.Host, _cfg.Port, _cfg.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto);
                await client.AuthenticateAsync(_cfg.User, _cfg.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    // Clase para almacenar la configuración SMTP,
    // con propiedades para el host, puerto, usuario, contraseña, nombre del remitente y si se debe usar StartTLS.
    public sealed class SmtpSettings
        {
            public string Host { get; set; } = "";
            public int Port { get; set; }
            public string User { get; set; } = "";
            public string Password { get; set; } = "";
            public string FromName { get; set; } = "Sistema";
            public bool UseStartTls { get; set; } = true;
        }
}
