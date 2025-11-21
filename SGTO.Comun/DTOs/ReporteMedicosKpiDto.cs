namespace SGTO.Comun.DTOs
{
    public class ReporteMedicosKpiDto
    {
        public int TotalMedicos { get; set; }
        public int Activos { get; set; }
        public int TotalTurnosRealizados { get; set; }
        public int ConMasPacientes { get; set; } 
        public int EspecialidadesCubiertas { get; set; }
    }
}