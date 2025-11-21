using System;

namespace SGTO.Comun.DTOs
{
    public class ReporteMedicosDto
    {
        public int IdMedico { get; set; }
        public string NombreCompleto { get; set; }
        public string Matricula { get; set; }
        public string Especialidad { get; set; }
        public int TotalTurnos { get; set; }
        public int PacientesAtendidos { get; set; } 
        public DateTime? UltimoTurno { get; set; }
        public string Estado { get; set; }
    }
}