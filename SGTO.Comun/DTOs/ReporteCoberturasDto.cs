namespace SGTO.Comun.DTOs
{
    public class ReporteCoberturasDto
    {
        public string Cobertura { get; set; }
        public int CantidadPlanes { get; set; }
        public int TotalTurnos { get; set; }
        public int PacientesAtendidos { get; set; }
        public string Estado { get; set; }
    }

    public class ReportePlanesDto
    {
        public string Cobertura { get; set; }
        public string Plan { get; set; }
        public decimal PorcentajeCubierto { get; set; }
        public int TotalTurnos { get; set; }
        public string Estado { get; set; }
    }
}