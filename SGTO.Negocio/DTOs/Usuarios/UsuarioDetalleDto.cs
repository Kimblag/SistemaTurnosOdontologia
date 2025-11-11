using System;

namespace SGTO.Negocio.DTOs.Usuarios
{
    public class UsuarioDetalleDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string NombreUsuario { get; set; }
        public string Rol { get; set; }
        public string Estado { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaModificacion { get; set; }

        // para el médico
        public string Dni { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string Telefono { get; set; }
        public string Matricula { get; set; }
        public string Especialidad { get; set; }
    }
}
