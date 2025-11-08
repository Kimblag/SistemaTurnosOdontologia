using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using System;

namespace SGTO.Dominio.Entidades
{
    public class Turno
    {
        public int IdTurno { get; set; }
        public Paciente Paciente { get; set; }
        public Medico Medico { get; set; }
        public Especialidad Especialidad { get; set; }
        public Tratamiento Tratamiento { get; set; }
        public HorarioTurno Horario { get; set; }
        public Cobertura Cobertura { get; set; }
        public Plan Plan { get; set; }
        public EstadoTurno Estado { get; set; }
        public string Observaciones { get; set; }

        public Turno()
        {
            
        }

        public Turno(Paciente paciente, Medico medico, Especialidad especialidad, Tratamiento tratamiento, HorarioTurno horario)
        {
            Paciente = paciente;
            Medico = medico;
            Especialidad = especialidad;
            Tratamiento = tratamiento;
            Horario = horario;

            Estado = EstadoTurno.Nuevo;
            Cobertura = paciente?.Cobertura;
            Plan = paciente?.Plan;
        }

        public void CambiarEstado(EstadoTurno nuevoEstado)
        {
            Estado = nuevoEstado;
        }

        public bool SolapaCon(Turno otroTurno)
        {
            return true;
        }

        public bool EsCancelable()
        {
            return true;
        }

        public HistoriaClinicaRegistro GenerarRegistroHistoria()
        {
            return new HistoriaClinicaRegistro();
        }
    }
}
