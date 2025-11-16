using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Turnos;
using System.Collections.Generic;


namespace SGTO.Negocio.Mappers
{
    public static class TurnoMapper
    {

        public static List<TurnoListadoDto> MapearListaTurnoListadoDto(List<Turno> turnos)
        {
            List<TurnoListadoDto> lista = new List<TurnoListadoDto>();

            foreach (Turno turno in turnos)
            {
                lista.Add(new TurnoListadoDto()
                {
                    IdTurno = turno.IdTurno,

                    NombrePaciente = turno.Paciente != null
                        ? $"{turno.Paciente.Apellido} {turno.Paciente.Nombre}"
                        : "Sin datos",

                    IdMedico = turno.Medico.IdMedico,
                    NombreMedico = turno.Medico != null
                        ? $"{turno.Medico.Apellido} {turno.Medico.Nombre}"
                        : "Sin datos",

                    IdEspecialidad = turno.Especialidad.IdEspecialidad,
                    Especialidad = turno.Especialidad?.Nombre ?? "-",

                    Fecha = turno.Horario != null
                        ? turno.Horario.Inicio.ToString("dd/MM/yyyy")
                        : "-",

                    Hora = turno.Horario != null
                        ? turno.Horario.Inicio.ToString("HH:mm")
                        : "-",

                    IdCobertura = turno.Cobertura.IdCobertura,
                    Cobertura = turno.Cobertura?.Nombre ?? "-",
                    IdPlan = (turno.Plan != null && turno.Plan.IdPlan != 0) ? turno.Plan.IdPlan : 0,
                    Plan = turno.Plan?.Nombre ?? "-",

                    Estado = turno.Estado.ToString()
                });
            }

            return lista;
        }


        public static List<TurnoPacienteDto> MapearListaTurnoPacienteDto(List<Turno> turnos)
        {
            List<TurnoPacienteDto> lista = new List<TurnoPacienteDto>();

            foreach (Turno turno in turnos)
            {
                lista.Add(new TurnoPacienteDto()
                {
                    IdTurnoPaciente = turno.IdTurno,
                    Fecha = turno.Horario.Inicio.ToString("dd/MM/yyyy"),
                    Hora = turno.Horario.Inicio.ToString("HH:mm"),
                    Medico = $"{turno.Medico.Apellido} {turno.Medico.Nombre}",
                    Observaciones = string.IsNullOrEmpty(turno.Observaciones) ? "-" : turno.Observaciones,
                    Estado = turno.Estado.ToString()
                });
            }
            return lista;
        }


    }
}
