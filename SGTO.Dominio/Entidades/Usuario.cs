using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Dominio.Entidades
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public Email Email { get; set; }
        public string NombreUsuario { get; set; }
        public string PasswordHash { get; set; }
        public Rol Rol { get; set; }
        public EstadoEntidad Estado { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaModificacion { get; set; }

        public Usuario()
        {
        }

        public Usuario(string nombre, string apellido, Email email,
            string nombreUsuario, string passwordHash, Rol rol)
        {
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            NombreUsuario = nombreUsuario;
            PasswordHash = passwordHash;
            Rol = rol;
            Estado = EstadoEntidad.Activo;
            FechaAlta = DateTime.Now;
            FechaModificacion = DateTime.Now;
        }
    }
}
