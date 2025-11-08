using System;

namespace SGTO.Negocio.DTOs.Pacientes
{
    public class PacienteCreacionDto
    {
        public string Dni { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Genero { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Estado { get; set; } // A, I
        public int IdCobertura { get; set; }
        public int IdPlan { get; set; } = 0;
    }
}
