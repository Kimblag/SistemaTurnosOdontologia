using System;

namespace SGTO.Negocio.DTOs.Pacientes
{
    public class PacienteEdicionDto
    {
        public int IdPaciente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; }  // M, F, O, N
        public string Telefono { get; set; }
        public string Email { get; set; }
        public int IdCobertura { get; set; }
        public int IdPlan { get; set; } = 0;
        public string Estado { get; set; }  // Activo, Inactivo

    }

}
