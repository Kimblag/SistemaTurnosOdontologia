using SGTO.Dominio.Enums;
using System;
using System.Collections.Generic;

namespace SGTO.Dominio.Entidades
{
    public class HorarioSemanalMedico
    {

        public int IdHorarioSemanal { get; set; }
        public Medico Medico { get; set; }

        // 1 = lunes ... 7 = domingo (colocamos igual que en la bd)
        public byte DiaSemana { get; set; }

        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }

        public EstadoEntidad Estado { get; set; } = EstadoEntidad.Activo;

        public HorarioSemanalMedico() { }

        public HorarioSemanalMedico(Medico medico, byte diaSemana, TimeSpan horaInicio, TimeSpan horaFin)
        {
            Medico = medico;
            DiaSemana = diaSemana;
            HoraInicio = horaInicio;
            HoraFin = horaFin;
            Estado = EstadoEntidad.Activo;
        }

        public bool EsRangoValido()
        {
            return HoraInicio < HoraFin;
        }
    }
}
