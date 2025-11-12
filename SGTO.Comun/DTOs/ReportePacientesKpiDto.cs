namespace SGTO.Comun.DTOs
{
    public class ReportePacientesKpiDto
    {
        public int TotalPacientes { get; set; }
        public int Atendidos { get; set; }
        public int NuevosEnPeriodo { get; set; }
        public int ConCobertura { get; set; }
        public int Particulares { get; set; }
    }
}
