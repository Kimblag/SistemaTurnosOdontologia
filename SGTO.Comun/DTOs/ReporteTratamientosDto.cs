using System;

namespace SGTO.Comun.DTOs
{
    public class ReporteTratamientosDto
    {
        public int IdTratamiento { get; set; }
        public string Nombre { get; set; }
        public string Especialidad { get; set; }
        public decimal CostoBase { get; set; }
        public int CantidadRealizados { get; set; } 
        public decimal IngresosEstimados { get; set; } 
        public string Estado { get; set; }
    }
}