using SGTO.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Dominio.Entidades
{
    public class Cobertura
    {
        public int IdCobertura { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public List<Plan> Planes { get; set; }
        public EstadoEntidad Estado { get; set; }


        public Cobertura(string nombre, string descripcion)
        {
            Nombre = nombre;
            Descripcion = !string.IsNullOrEmpty(descripcion) ? descripcion : string.Empty; ;
            Estado = EstadoEntidad.Activo;
        }

        public Cobertura(int idCobertura, string nombre, string descripcion, EstadoEntidad estado)
        {
            IdCobertura = idCobertura;
            Nombre = nombre;
            Descripcion = !string.IsNullOrEmpty(descripcion) ? descripcion : string.Empty;
            Estado = estado;
        }

        public Cobertura(int idCobertura, string nombre, string descripcion, List<Plan> planes, EstadoEntidad estado)
        {
            IdCobertura = idCobertura;
            Nombre = nombre;
            Descripcion = !string.IsNullOrEmpty(descripcion) ? descripcion : string.Empty;
            Planes = planes;
            Estado = estado;
        }
    }
}
