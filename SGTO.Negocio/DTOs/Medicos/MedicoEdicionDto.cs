using System;

namespace SGTO.Negocio.DTOs.Medicos
{
    public class MedicoEdicionDto
    {
        public int IdUsuario { get; set; }
        public int IdMedico { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; } // M, F, O, N
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Matricula { get; set; }
        public int IdEspecialidad { get; set; }
        public string Estado { get; set; } // A, I
    }
}
