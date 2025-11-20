using System;

namespace SGTO.Negocio.DTOs.Turnos
{
    public class TurnoDetalleDto
    {
        public int IdTurno { get; set; }
        public string NombrePaciente { get; set; }
        public int IdPaciente { get; set; }
        public string NombreMedico { get; set; }
        public int IdMedico { get; set; }
        public string Especialidad { get; set; }
        public int IdEspecialidad { get; set; }
        public string Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Cobertura { get; set; }
        public int IdCobertura { get; set; }
        public string Plan { get; set; }
        public int IdPlan { get; set; }
        public string Observaciones { get; set; }

        // datos de la historia clinca si existen (turnos cerrados)
        public string Diagnostico { get; set; }
        public string TratamientoAplicado { get; set; }
        public string ObservacionesClinicas { get; set; }
    }
}
