namespace SGTO.Comun.DTOs
{
    public class ReporteCoberturasKpiDto
    {
        public int TotalCoberturas { get; set; }
        public int TotalPlanes { get; set; }
        public string CoberturaMasUsada { get; set; }
        public decimal TotalFacturado { get; set; }
        public decimal TotalACobrarOS { get; set; }
        public decimal TotalCopagos { get; set; }
    }
}