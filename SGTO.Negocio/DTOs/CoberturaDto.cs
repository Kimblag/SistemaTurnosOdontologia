using System;
using System.Collections.Generic;


namespace SGTO.Negocio.DTOs
{
    public class CoberturaDto
    {
        public int IdCobertura { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int CantidadPlanes { get; set; }
        public decimal? PorcentajeCobertura { get; set; } = null;
        public string Estado { get; set; } // activo - inactivo
        public List<string> NombrePlanes { get; set; } = new List<string>();
    }
}
