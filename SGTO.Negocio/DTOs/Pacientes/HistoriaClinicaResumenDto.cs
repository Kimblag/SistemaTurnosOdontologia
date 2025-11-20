using System;

namespace SGTO.Negocio.DTOs.Pacientes
{
    public class HistoriaClinicaResumenDto
    {
        public int IdTurno { get; set; }

        public DateTime Fecha { get; set; }
        public string Tratamiento { get; set; }
        public string Diagnostico { get; set; }

        public string Profesional { get; set; }
        public string Especialidad { get; set; }
    }
}
