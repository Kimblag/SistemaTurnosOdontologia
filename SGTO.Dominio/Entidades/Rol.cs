using SGTO.Dominio.Enums;
using System.Collections.Generic;

namespace SGTO.Dominio.Entidades
{
    public class Rol
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public List<Permiso> Permisos { get; set; }
        public EstadoEntidad Estado { get; set; }

        public Rol()
        {
            
        }

        public Rol(string nombre, string descripcion, List<Permiso> permisos)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Permisos = permisos;
            Estado = EstadoEntidad.Activo;
        }

        public Rol(int idRol, string nombre, string descripcion,
            List<Permiso> permisos, EstadoEntidad estado)
        {
            IdRol = idRol;
            Nombre = nombre;
            Descripcion = descripcion;
            Permisos = permisos;
            Estado = estado;
        }
    }
}
