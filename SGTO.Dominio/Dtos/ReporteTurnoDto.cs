using System;

namespace SGTO.Dominio.Dtos
{
    public class ReporteTurnoDto
    {
        public DateTime Fecha { get; set; }
        public string Paciente { get; set; }
        public string Medico { get; set; }
        public string Especialidad { get; set; }
        public string Cobertura { get; set; }
        public string Plan { get; set; }
        public string Estado { get; set; }
    }
}
