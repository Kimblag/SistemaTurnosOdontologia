using SGTO.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Dominio.Entidades
{
    public class Rol
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public List<Permiso> Permisos { get; set; }
        public EstadoEntidad Estado { get; set; }

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
