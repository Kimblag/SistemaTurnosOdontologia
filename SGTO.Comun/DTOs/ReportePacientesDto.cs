using System;

namespace SGTO.Comun.DTOs
{
    public class ReportePacientesDto
    {
        public int IdPaciente { get; set; }
        public string NombreCompleto { get; set; }
        public string NumeroDocumento { get; set; }
        public string Cobertura { get; set; }
        public string Plan { get; set; }
        public int TotalTurnos { get; set; }
        public DateTime? UltimaAtencion { get; set; }
        public string MedicoFrecuente { get; set; }
    }
}
