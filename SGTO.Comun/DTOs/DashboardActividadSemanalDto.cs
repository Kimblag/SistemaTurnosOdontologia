using System;

namespace SGTO.Comun.DTOs
{
    public class DashboardActividadSemanalDto
    {
        public string Dia { get; set; }
        public DateTime Fecha { get; set; }
        public int CantidadNuevos { get; set; }
        public int CantidadReprogramados { get; set; }
        public int CantidadCerrados { get; set; }
        public int CantidadCancelados { get; set; }
    }
}
