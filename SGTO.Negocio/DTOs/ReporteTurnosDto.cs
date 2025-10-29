using System;

namespace SGTO.Negocio.DTOs
{
    public class ReporteTurnosDto
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
