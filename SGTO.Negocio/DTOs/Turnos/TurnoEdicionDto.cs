using System;

namespace SGTO.Negocio.DTOs.Turnos
{
    public class TurnoEdicionDto
    {
        public int IdTurno { get; set; }

        public int IdPaciente { get; set; }
        public string NombreCompletoPaciente { get; set; }

        public int IdEspecialidad { get; set; }
        public int IdMedico { get; set; }
        public int IdCobertura { get; set; }
        public int IdPlan { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public char Estado { get; set; }
        public string Observaciones { get; set; }
    }
}
