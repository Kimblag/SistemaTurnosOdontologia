using SGTO.Dominio.Enums;
using System;
namespace SGTO.Dominio.Entidades
{
    public class AgendaMedico
    {
        public int IdAgendaMedico { get; set; }
        public Medico Medico { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public EstadoAgendaMedico Estado { get; set; } = EstadoAgendaMedico.Libre;

        public Turno Turno { get; set; }
        public string Observacion { get; set; }
    }
}
