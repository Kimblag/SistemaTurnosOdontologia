namespace SGTO.Negocio.DTOs.HistoriaClinica
{
    public class HistoriaClinicaCreacionDto
    {
        public int IdTurno { get; set; }
        public int IdTratamiento { get; set; } = 0;
        public string TratamientoManual { get; set; }
        public string Diagnostico { get; set; }
        public string Observaciones { get; set; }
    }
}
