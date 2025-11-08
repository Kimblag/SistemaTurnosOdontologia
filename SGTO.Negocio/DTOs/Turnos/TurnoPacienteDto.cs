
namespace SGTO.Negocio.DTOs.Turnos
{
    public class TurnoPacienteDto
    {
        public int IdTurnoPaciente { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Medico { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; }
    }
}
