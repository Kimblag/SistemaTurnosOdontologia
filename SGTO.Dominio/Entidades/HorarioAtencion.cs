using SGTO.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Dominio.Entidades
{
    public class HorarioAtencion
    {
        public int IdHorarioAtencion { get; set; }
        public Medico Medico { get; set; }
        public string DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public EstadoEntidad Estado { get; set; } = EstadoEntidad.Activo;

        public HorarioAtencion(Medico medico, string diaSemana,
            TimeSpan horaInicio, TimeSpan horaFin)
        {
            Medico = medico;
            DiaSemana = diaSemana;
            HoraInicio = horaInicio;
            HoraFin = horaFin;
            Estado = EstadoEntidad.Activo;
        }

        public HorarioAtencion(int idHorarioAtencion, Medico medico, string diaSemana,
            TimeSpan horaInicio, TimeSpan horaFin, EstadoEntidad estado)
        {
            IdHorarioAtencion = idHorarioAtencion;
            Medico = medico;
            DiaSemana = diaSemana;
            HoraInicio = horaInicio;
            HoraFin = horaFin;
            Estado = estado;
        }

        public bool EsValido()
        {
            return true;
        }

        public bool SolapaCon(HorarioAtencion otro)
        {
            return false;
        }

    }
}
