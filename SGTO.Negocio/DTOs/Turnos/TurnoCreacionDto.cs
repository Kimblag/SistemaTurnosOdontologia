using System;

namespace SGTO.Negocio.DTOs.Turnos
{
    public class TurnoCreacionDto
    {
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public int IdEspecialidad { get; set; }

        public int IdCobertura { get; set; }
        public int IdPlan { get; set; } = 0;

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public char Estado { get; set; } // 'N', 'P', 'R', 'X', 'C', 'Z'

        public string Observaciones { get; set; }
    }
}
