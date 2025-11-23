using System.Collections.Generic;

namespace SGTO.Negocio.DTOs.Seguridad
{
    public class UsuarioSesionDto
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string NombreCompleto { get; set; }


        public int IdRol { get; set; }
        public string NombreRol { get; set; }
        public bool EsAdmin { get; set; }

       
        public int? IdMedico { get; set; }

        // mostraremos permosos en forma de strings como "TURNOS_VER", "PACIENTES_EDITAR" para poder saber rapidamente el modulo y el permiso que tiene.
        public List<string> Permisos { get; set; }

        public UsuarioSesionDto()
        {
            Permisos = new List<string>();
        }
    }
}
