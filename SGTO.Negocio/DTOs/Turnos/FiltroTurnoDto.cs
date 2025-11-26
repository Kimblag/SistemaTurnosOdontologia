using System;

namespace SGTO.Negocio.DTOs.Turnos
{
    public class FiltroTurnoDto
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? IdMedico { get; set; }
        public int? IdPaciente { get; set; }
        public int? IdCobertura { get; set; }
        public int? IdEspecialidad { get; set; }
        public char? Estado { get; set; }
    }
}
