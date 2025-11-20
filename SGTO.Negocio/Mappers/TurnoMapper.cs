using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Turnos;
using System;
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

                    DniPaciente = turno.Paciente != null
                        ? turno.Paciente.Dni.Numero
                        : "Sin datos",
                    NombrePaciente = turno.Paciente != null
                        ? $"{turno.Paciente.Apellido} {turno.Paciente.Nombre}"
                        : "Sin datos",

                    IdMedico = turno.Medico.IdMedico,
                    NombreMedico = turno.Medico != null
                        ? $"{turno.Medico.Apellido} {turno.Medico.Nombre}"
                        : "Sin datos",
                    Matricula = turno.Medico != null
                        ? turno.Medico.Matricula
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

        public static Turno MapearACreacion(TurnoCreacionDto dto)
        {
            if (dto == null)
                return null;

            return new Turno
            {
                Paciente = new Paciente { IdPaciente = dto.IdPaciente },
                Medico = new Medico { IdMedico = dto.IdMedico },
                Especialidad = new Especialidad { IdEspecialidad = dto.IdEspecialidad },

                Cobertura = new Cobertura { IdCobertura = dto.IdCobertura },
                Plan = dto.IdPlan != 0
                    ? new Plan { IdPlan = dto.IdPlan }
                    : null,

                Horario = new HorarioTurno(dto.FechaInicio, dto.FechaFin, validar: true),

                Estado = (EstadoTurno)dto.Estado,

                Observaciones = dto.Observaciones
            };
        }


        public static TurnoEdicionDto MapearAEdicionDto(Turno turno)
        {
            if (turno == null) return null;

            TurnoEdicionDto dto = new TurnoEdicionDto();

            dto.IdTurno = turno.IdTurno;
            dto.FechaInicio = turno.Horario.Inicio;
            dto.FechaFin = turno.Horario.Fin;
            dto.Estado = turno.Estado.ToString()[0];

            dto.IdPaciente = turno.Paciente.IdPaciente;
            dto.NombreCompletoPaciente = $"{turno.Paciente.Apellido}, {turno.Paciente.Nombre}";

            dto.IdEspecialidad = turno.Especialidad.IdEspecialidad;

            dto.IdMedico = turno.Medico.IdMedico;

            dto.IdCobertura = turno.Cobertura.IdCobertura;

            if (turno.Plan != null)
                dto.IdPlan = turno.Plan.IdPlan;

            dto.Observaciones = turno.Observaciones;
            return dto;
        }


        public static void MapearEdicion(Turno turno, TurnoEdicionDto dto)
        {
            if (turno == null || dto == null)
                return;

            turno.Paciente = new Paciente { IdPaciente = dto.IdPaciente };
            turno.Medico = new Medico { IdMedico = dto.IdMedico };
            turno.Especialidad = new Especialidad { IdEspecialidad = dto.IdEspecialidad };
            turno.Cobertura = new Cobertura { IdCobertura = dto.IdCobertura };
            turno.Plan = dto.IdPlan != 0 ? new Plan { IdPlan = dto.IdPlan } : null;
            turno.Horario = new HorarioTurno(dto.FechaInicio, dto.FechaFin, validar: true);
            turno.Estado = (EstadoTurno)dto.Estado; // ojo con esto, si dto.Estado sigue siendo char hay que convertir
            turno.Observaciones = dto.Observaciones;
        }



        public static TurnoDetalleDto MapearADetalleDto(Turno turno)
        {
            if (turno == null) return null;

            return new TurnoDetalleDto
            {
                IdTurno = turno.IdTurno,

                IdPaciente = turno.Paciente?.IdPaciente ?? 0,
                NombrePaciente = $"{turno.Paciente.Apellido}, {turno.Paciente.Nombre}",

                IdMedico = turno.Medico?.IdMedico ?? 0,
                NombreMedico = $"{turno.Medico.Apellido}, {turno.Medico.Nombre}",

                IdEspecialidad = turno.Especialidad?.IdEspecialidad ?? 0,
                Especialidad = turno.Especialidad.Nombre,

                Estado = EnumeracionMapperNegocio.ObtenerNombreEstadoTurno(EnumeracionMapperNegocio.ObtenerChar(turno.Estado)),

                FechaInicio = turno.Horario.Inicio,
                FechaFin = turno.Horario.Fin,

                IdCobertura = turno.Cobertura.IdCobertura,
                Cobertura = turno.Cobertura.Nombre,

                IdPlan = (turno.Plan != null && turno.Plan.IdPlan != 0) ? turno.Plan.IdPlan : 0,
                Plan = turno.Plan?.Nombre ?? "-",

                Observaciones = turno.Observaciones ?? "-"
            };
        }


    }
}
