using System;

namespace SGTO.Negocio.DTOs.Usuarios
{
    public class UsuarioEdicionDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; } = string.Empty;
        public string ConfirmarPassword { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public string Estado { get; set; } // A, I

        // si es médico
        public string Dni { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string Telefono { get; set; }
        public string Matricula { get; set; }
        public int IdEspecialidad { get; set; }
    }
}
