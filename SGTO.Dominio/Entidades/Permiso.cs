using SGTO.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Dominio.Entidades
{
    public class Permiso
    {
        public int IdPermiso { get; set; }
        public Modulo Modulo { get; set; }
        public TipoAccion Accion { get; set; }
        public string Descripcion { get; set; } = string.Empty;


        public Permiso(Modulo modulo, TipoAccion accion, string descripcion)
        {
            Modulo = modulo;
            Accion = accion;
            Descripcion = descripcion;
        }

        public Permiso(int idPermiso, Modulo modulo, TipoAccion accion, string descripcion)
        {
            IdPermiso = idPermiso;
            Modulo = modulo;
            Accion = accion;
            Descripcion = string.IsNullOrEmpty(descripcion) ? string.Empty : descripcion;
        }

        public string Clave()
        {
            return $"{Modulo.ToString().ToUpper()}_{Accion.ToString().ToUpper()}";
        }

    }
}
