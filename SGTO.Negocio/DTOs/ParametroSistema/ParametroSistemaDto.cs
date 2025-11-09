namespace SGTO.Negocio.DTOs.ParametroSistema
{
    public class ParametroSistemaDto
    {
        public string NombreClinica { get; set; }
        public int DuracionTurnoMinutos { get; set; }
        public string HoraInicio { get; set; }
        public string HoraCierre { get; set; }
        public string ServidorCorreo { get; set; }
        public int PuertoCorreo { get; set; }
        public string EmailRemitente { get; set; }
        public int ReintentosEmail { get; set; }
        public string Moneda { get; set; }

    }
}
