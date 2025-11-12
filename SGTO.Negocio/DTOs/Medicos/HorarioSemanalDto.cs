using System;

namespace SGTO.Negocio.DTOs.Medicos
{
    public class HorarioSemanalDto
    {
        public byte DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string Estado { get; set; } = "A";
    }
}
