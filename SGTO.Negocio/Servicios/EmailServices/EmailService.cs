using SGTO.Datos.Repositorios;
using SGTO.Dominio.Entidades;
using System;
using System.Net;
using System.Net.Mail;


namespace SGTO.Negocio.Servicios.EmailServices
{
    public class EmailService
    {
        private string _servidorSmtp;
        private int _puertoSmtp;
        private string _emailFrom;
        private string _nombreClinica;
        private string _usuarioSmtp;
        private int _reintentos;

        private const string PASSWORD_SMTP = "PASS";

        private ParametroSistemaRepositorio _repoParametros;

        public EmailService()
        {
            _repoParametros = new ParametroSistemaRepositorio();

            _servidorSmtp = _repoParametros.ObtenerValor("SMTP_Server");
            _puertoSmtp = Convert.ToInt32(_repoParametros.ObtenerValor("SMTP_Port"));
            _emailFrom = _repoParametros.ObtenerValor("Email_From");
            _nombreClinica = _repoParametros.ObtenerValor("NombreClinica");
            _usuarioSmtp = _repoParametros.ObtenerValor("UsuarioCorreo");

            string valorReintentos = _repoParametros.ObtenerValor("ReintentosEmail");
            _reintentos = string.IsNullOrEmpty(valorReintentos)
                ? 1
                : Convert.ToInt32(valorReintentos);
        }

        private string Reemplazar(string texto, string token, string valor)
        {
            if (texto == null) return string.Empty;
            if (token == null) return texto;
            if (valor == null) valor = string.Empty;

            return texto.Replace(token, valor);
        }

        public string GenerarHtmlConfirmacion(
            string html,
            Paciente paciente,
            Medico medico,
            Especialidad esp,
            DateTime fechaInicio,
            string observaciones)
        {
            DateTime fechaFin = fechaInicio.AddHours(1);

            string fechaInicioUtc = fechaInicio.ToUniversalTime().ToString("yyyyMMddTHHmmssZ");
            string fechaFinUtc = fechaFin.ToUniversalTime().ToString("yyyyMMddTHHmmssZ");

            string pacienteNombre = paciente.NombreCompleto();
            string medicoNombre = medico.NombreCompleto();
            string especialidadNombre = esp.Nombre;

            string titulo = Uri.EscapeDataString($"Turno médico - {especialidadNombre}");
            string detalles = Uri.EscapeDataString($"Cita con {medicoNombre}");
            string ubicacion = Uri.EscapeDataString(_nombreClinica);

            string googleCalendarUrl =
                $"https://calendar.google.com/calendar/render?action=TEMPLATE" +
                $"&text={titulo}" +
                $"&dates={fechaInicioUtc}/{fechaFinUtc}" +
                $"&details={detalles}" +
                $"&location={ubicacion}";

            html = Reemplazar(html, "{{NombreClinica}}", _nombreClinica);
            html = Reemplazar(html, "{{Paciente}}", pacienteNombre);
            html = Reemplazar(html, "{{Medico}}", medicoNombre);
            html = Reemplazar(html, "{{Especialidad}}", esp.Nombre);
            html = Reemplazar(html, "{{Fecha}}", fechaInicio.ToString("dd/MM/yyyy"));
            html = Reemplazar(html, "{{Hora}}", fechaInicio.ToString("HH:mm"));
            html = Reemplazar(html, "{{Observaciones}}", string.IsNullOrWhiteSpace(observaciones) ? "-" : observaciones);
            html = Reemplazar(html, "{{LinkAgregarACalendario}}", googleCalendarUrl);

            return html;
        }

        public bool Enviar(string destinatario, string asunto, string htmlCuerpo)
        {
            int intentos = 0;
            bool enviado = false;

            while (!enviado && intentos < _reintentos)
            {
                intentos++;

                MailMessage mensaje = null;
                SmtpClient cliente = null;

                try
                {
                    mensaje = new MailMessage();
                    mensaje.From = new MailAddress(_emailFrom, _nombreClinica);
                    mensaje.To.Add(destinatario);
                    mensaje.Subject = asunto;
                    mensaje.Body = htmlCuerpo;
                    mensaje.IsBodyHtml = true;

                    cliente = new SmtpClient(_servidorSmtp, _puertoSmtp);
                    cliente.Credentials = new NetworkCredential(_usuarioSmtp, PASSWORD_SMTP);
                    cliente.EnableSsl = true;

                    cliente.Send(mensaje);
                    enviado = true;
                }
                catch
                {
                    enviado = false;
                    if (intentos >= _reintentos)
                        return false;
                }
                finally
                {
                    if (mensaje != null) mensaje.Dispose();
                    if (cliente != null) cliente.Dispose();
                }
            }

            return enviado;
        }
    }
}
