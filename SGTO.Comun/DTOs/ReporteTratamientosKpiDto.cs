namespace SGTO.Comun.DTOs
{
    public class ReporteTratamientosKpiDto
    {
        public int TotalEnCatalogo { get; set; }
        public int TotalRealizados { get; set; }
        public decimal TotalFacturado { get; set; } 
        public decimal TotalCobradoObraSocial { get; set; }
        public decimal TotalCobradoPaciente { get; set; }

        public string TratamientoMasSolicitado { get; set; }
        public string EspecialidadMasDemandada { get; set; }
    }
}