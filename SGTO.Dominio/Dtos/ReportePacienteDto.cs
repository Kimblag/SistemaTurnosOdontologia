using System;

namespace SGTO.Dominio.Dtos
{
    public class ReportePacienteDto
    {
        public string Nombre { get; set; }
        public string Dni { get; set; }
        public string Cobertura { get; set; }
        public string Plan { get; set; }
        public int TotalTurnos { get; set; }
        public DateTime UltimaAtencion { get; set; }
        public string MedicoMasFrecuente { get; set; }
    }
}
