using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Dominio.Entidades
{
    public class HistoriaClinicaRegistro
    {
        public int IdHistoriaClinicaRegistro { get; set; }
        public DateTime FechaAtencion { get; set; }
        public Medico Medico { get; set; }
        public Especialidad Especialidad { get; set; }
        public Tratamiento TratamientoAplicado { get; set; }
        public string Diagnostico { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public Turno TurnoOrigen { get; set; }


        public HistoriaClinicaRegistro()
        {
            
        }

        public HistoriaClinicaRegistro(DateTime fechaAtencion, Medico medico,
            Especialidad especialidad, Tratamiento tratamientoAplicado,
            string diagnostico, string observaciones, Turno turnoOrigen)
        {
            FechaAtencion = fechaAtencion;
            Medico = medico;
            Especialidad = especialidad;
            TratamientoAplicado = tratamientoAplicado;
            Diagnostico = diagnostico;
            Observaciones = observaciones;
            TurnoOrigen = turnoOrigen;
        }

        public HistoriaClinicaRegistro(int idHistoriaClinicaRegistro, DateTime fechaAtencion,
            Medico medico, Especialidad especialidad, Tratamiento tratamientoAplicado,
            string diagnostico, string observaciones, Turno turnoOrigen)
        {
            IdHistoriaClinicaRegistro = idHistoriaClinicaRegistro;
            FechaAtencion = fechaAtencion;
            Medico = medico;
            Especialidad = especialidad;
            TratamientoAplicado = tratamientoAplicado;
            Diagnostico = diagnostico;
            Observaciones = observaciones;
            TurnoOrigen = turnoOrigen;
        }

    }
}
