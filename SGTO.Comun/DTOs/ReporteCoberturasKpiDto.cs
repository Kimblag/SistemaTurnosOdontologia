namespace SGTO.Comun.DTOs
{
    public class ReporteCoberturasKpiDto
    {
        public int TotalCoberturas { get; set; }
        public int TotalPlanes { get; set; }
        public int TurnosPorObraSocial { get; set; } 
        public string CoberturaMasUsada { get; set; }
    }
}