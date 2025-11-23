using System;

namespace SGTO.Comun.DTOs
{
    public class ReporteTurnosDto
    {
        public int IdTurno { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public string Paciente { get; set; }
        public string DniPaciente { get; set; }
        public string Medico { get; set; }
        public string Especialidad { get; set; }
        public string Estado { get; set; }
        public string Cobertura { get; set; }
        public string Plan { get; set; }
    }
}
