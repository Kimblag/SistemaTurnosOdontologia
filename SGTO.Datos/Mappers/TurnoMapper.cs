using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Datos.Mappers
{
    public static class TurnoMapper
    {

        public static Turno MapearAEntidadBasico(SqlDataReader lector)
        {
            Turno turno = new Turno()
            {
                IdTurno = lector.GetInt32(lector.GetOrdinal("IdTurno")),
                Observaciones = lector.IsDBNull(lector.GetOrdinal("Observaciones")) ? string.Empty : lector.GetString(lector.GetOrdinal("Observaciones")),
                Estado = EnumeracionMapperDatos.MapearEstadoTurno(lector, "EstadoTurno"),
                Horario = new HorarioTurno(lector.GetDateTime(lector.GetOrdinal("FechaInicio")),
                lector.GetDateTime(lector.GetOrdinal("FechaFin")), false),
                Medico = new Medico()
                {
                    IdMedico = lector.GetInt32(lector.GetOrdinal("IdMedico")),
                    Nombre = lector.GetString(lector.GetOrdinal("NombreMedico")),
                    Apellido = lector.GetString(lector.GetOrdinal("ApellidoMedico")),
                },
                Especialidad = new Especialidad()
                {
                    IdEspecialidad = lector.GetInt32(lector.GetOrdinal("IdEspecialidad")),
                    Nombre = lector.GetString(lector.GetOrdinal("NombreEspecialidad"))
                },
            };
            return turno;
        }

        public static Turno MapearAEntidadCompleto(SqlDataReader lector)
        {
            var turno = MapearAEntidadBasico(lector);

            turno.Paciente = new Paciente
            {
                IdPaciente = lector.GetInt32(lector.GetOrdinal("IdPaciente")),
                Nombre = lector.GetString(lector.GetOrdinal("NombrePaciente")),
                Apellido = lector.GetString(lector.GetOrdinal("ApellidoPaciente")),
                Dni = new DocumentoIdentidad(lector.GetString(lector.GetOrdinal("NumeroDocumento"))),
                Telefono = new Telefono(lector.GetString(lector.GetOrdinal("Telefono"))),
                Email = new Email(lector.GetString(lector.GetOrdinal("Email"))),
                Cobertura = new Cobertura
                {
                    IdCobertura = lector.GetInt32(lector.GetOrdinal("IdCobertura")),
                    Nombre = lector.GetString(lector.GetOrdinal("NombreCobertura"))
                },
                Plan = lector.IsDBNull(lector.GetOrdinal("IdPlan")) ? null : new Plan
                {
                    IdPlan = lector.GetInt32(lector.GetOrdinal("IdPlan")),
                    Nombre = lector.GetString(lector.GetOrdinal("NombrePlan"))
                }
            };

            turno.Cobertura = turno.Paciente.Cobertura;
            turno.Plan = turno.Paciente.Plan;

            return turno;
        }


        public static Turno MapearAEntidadListado(SqlDataReader lector)
        {
            int idTurno = lector.GetInt32(lector.GetOrdinal("IdTurno"));
            DateTime fechaInicio = lector.GetDateTime(lector.GetOrdinal("FechaInicio"));
            DateTime fechaFin = lector.GetDateTime(lector.GetOrdinal("FechaFin"));
            EstadoTurno estado = EnumeracionMapperDatos.MapearEstadoTurno(lector, "EstadoTurno");

            string observaciones = lector.IsDBNull(lector.GetOrdinal("Observaciones"))
                ? null
                : lector.GetString(lector.GetOrdinal("Observaciones"));

            Paciente paciente = new Paciente()
            {
                IdPaciente = lector.GetInt32(lector.GetOrdinal("IdPaciente")),
                Nombre = lector.GetString(lector.GetOrdinal("NombrePaciente")),
                Apellido = lector.GetString(lector.GetOrdinal("ApellidoPaciente"))
            };

            Medico medico = new Medico()
            {
                IdMedico = lector.GetInt32(lector.GetOrdinal("IdMedico")),
                Nombre = lector.GetString(lector.GetOrdinal("NombreMedico")),
                Apellido = lector.GetString(lector.GetOrdinal("ApellidoMedico"))
            };

            Especialidad especialidad = new Especialidad()
            {
                IdEspecialidad = lector.GetInt32(lector.GetOrdinal("IdEspecialidad")),
                Nombre = lector.GetString(lector.GetOrdinal("NombreEspecialidad"))
            };

            Cobertura cobertura = new Cobertura()
            {
                IdCobertura = lector.GetInt32(lector.GetOrdinal("IdCobertura")),
                Nombre = lector.GetString(lector.GetOrdinal("NombreCobertura"))
            };

            Plan plan = null;
            int idPlan = lector.GetOrdinal("IdPlan");
            if (!lector.IsDBNull(idPlan))
            {
                plan = new Plan()
                {
                    IdPlan = lector.GetInt32(idPlan),
                    Nombre = lector.GetString(lector.GetOrdinal("NombrePlan"))
                };
            }

            HorarioTurno horario = new HorarioTurno(fechaInicio, fechaFin, validar: false);

            var turno = new Turno()
            {
                IdTurno = idTurno,
                Paciente = paciente,
                Medico = medico,
                Especialidad = especialidad,
                Cobertura = cobertura,
                Plan = plan,
                Horario = horario,
                Estado = estado,
                Observaciones = observaciones
            };

            return turno;
        }


    }
}
