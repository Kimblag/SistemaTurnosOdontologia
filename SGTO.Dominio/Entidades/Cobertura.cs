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
        public decimal? PorcentajeCobertura { get; set; } = null;
        public List<Plan> Planes { get; set; }
        public EstadoEntidad Estado { get; set; }

        public Cobertura()
        {

        }

        public Cobertura(string nombre, string descripcion)
        {
            Nombre = nombre;
            Descripcion = !string.IsNullOrEmpty(descripcion) ? descripcion : string.Empty;
            Planes = new List<Plan>();
            Estado = EstadoEntidad.Activo;
            PorcentajeCobertura = 0;
        }

        public Cobertura(int idCobertura, string nombre, string descripcion, decimal? porcentajeCobertura, EstadoEntidad estado)
        {
            IdCobertura = idCobertura;
            Nombre = nombre;
            Descripcion = !string.IsNullOrEmpty(descripcion) ? descripcion : string.Empty;
            Planes = new List<Plan>();
            PorcentajeCobertura = porcentajeCobertura;
            Estado = estado;
        }

        public Cobertura(int idCobertura, string nombre, string descripcion, decimal? porcentajeCobertura, List<Plan> planes, EstadoEntidad estado)
        {
            IdCobertura = idCobertura;
            Nombre = nombre;
            Descripcion = !string.IsNullOrEmpty(descripcion) ? descripcion : string.Empty;
            Planes = planes;
            PorcentajeCobertura = porcentajeCobertura;
            Estado = estado;
        }
    }
}
