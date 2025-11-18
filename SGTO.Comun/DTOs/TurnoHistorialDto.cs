using System;

namespace SGTO.Comun.DTOs
{
    public class TurnoHistorialDto
    {
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Paciente { get; set; }
        public string Tratamiento { get; set; }
        public string Cobertura { get; set; }
        public string Estado { get; set; }
    }
}