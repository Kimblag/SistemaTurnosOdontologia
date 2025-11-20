using SGTO.Dominio.Entidades;
using SGTO.Dominio.ObjetosValor;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SGTO.Datos.Mappers
{
    public static class TurnoMapper
    {

        public static Turno MapearAEntidadBasico(SqlDataReader lector)
        {
            var turno = new Turno();

            if (TieneColumna(lector, "IdTurno"))
                turno.IdTurno = lector.GetInt32(lector.GetOrdinal("IdTurno"));

            if (TieneColumna(lector, "Estado") || TieneColumna(lector, "EstadoTurno"))
                turno.Estado = EnumeracionMapperDatos.MapearEstadoTurno(
                    lector,
                    TieneColumna(lector, "EstadoTurno") ? "EstadoTurno" : "Estado"
                );

            if (TieneColumna(lector, "Observaciones"))
                turno.Observaciones = lector.IsDBNull(lector.GetOrdinal("Observaciones"))
                    ? null
                    : lector.GetString(lector.GetOrdinal("Observaciones"));

            DateTime fechaInicio = DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;

            if (TieneColumna(lector, "FechaInicio"))
                fechaInicio = lector.GetDateTime(lector.GetOrdinal("FechaInicio"));
            if (TieneColumna(lector, "FechaFin"))
                fechaFin = lector.GetDateTime(lector.GetOrdinal("FechaFin"));

            if (fechaInicio != DateTime.MinValue && fechaFin != DateTime.MinValue)
                turno.Horario = new HorarioTurno(fechaInicio, fechaFin, validar: false);


            if (TieneColumna(lector, "IdMedico"))
            {
                turno.Medico = new Medico { IdMedico = lector.GetInt32(lector.GetOrdinal("IdMedico")) };

                if (TieneColumna(lector, "NombreMedico"))
                    turno.Medico.Nombre = lector.GetString(lector.GetOrdinal("NombreMedico"));

                if (TieneColumna(lector, "ApellidoMedico"))
                    turno.Medico.Apellido = lector.GetString(lector.GetOrdinal("ApellidoMedico"));
            }


            if (TieneColumna(lector, "IdEspecialidad"))
            {
                turno.Especialidad = new Especialidad { IdEspecialidad = lector.GetInt32(lector.GetOrdinal("IdEspecialidad")) };

                if (TieneColumna(lector, "NombreEspecialidad"))
                    turno.Especialidad.Nombre = lector.GetString(lector.GetOrdinal("NombreEspecialidad"));
            }

            if (TieneColumna(lector, "IdPaciente"))
            {
                turno.Paciente = new Paciente { IdPaciente = lector.GetInt32(lector.GetOrdinal("IdPaciente")) };
            }

            return turno;
        }


        public static Turno MapearAEntidadCompleto(SqlDataReader lector)
        {
            var turno = MapearAEntidadBasico(lector);

            if (TieneColumna(lector, "IdPaciente"))
            {
                var p = new Paciente
                {
                    IdPaciente = lector.GetInt32(lector.GetOrdinal("IdPaciente"))
                };

                if (TieneColumna(lector, "NombrePaciente"))
                    p.Nombre = lector.GetString(lector.GetOrdinal("NombrePaciente"));

                if (TieneColumna(lector, "ApellidoPaciente"))
                    p.Apellido = lector.GetString(lector.GetOrdinal("ApellidoPaciente"));

                if (TieneColumna(lector, "NumeroDocumento"))
                    p.Dni = new DocumentoIdentidad(lector.GetString(lector.GetOrdinal("NumeroDocumento")));

                if (TieneColumna(lector, "Telefono"))
                    p.Telefono = new Telefono(lector.GetString(lector.GetOrdinal("Telefono")));

                if (TieneColumna(lector, "Email"))
                    p.Email = new Email(lector.GetString(lector.GetOrdinal("Email")));

                if (TieneColumna(lector, "IdCobertura"))
                    p.Cobertura = new Cobertura
                    {
                        IdCobertura = lector.GetInt32(lector.GetOrdinal("IdCobertura")),
                        Nombre = TieneColumna(lector, "NombreCobertura")
                            ? lector.GetString(lector.GetOrdinal("NombreCobertura"))
                            : null
                    };

                if (TieneColumna(lector, "IdPlan") && !lector.IsDBNull(lector.GetOrdinal("IdPlan")))
                    p.Plan = new Plan
                    {
                        IdPlan = lector.GetInt32(lector.GetOrdinal("IdPlan")),
                        Nombre = TieneColumna(lector, "NombrePlan")
                            ? lector.GetString(lector.GetOrdinal("NombrePlan"))
                            : null
                    };

                turno.Paciente = p;
                turno.Cobertura = p.Cobertura;
                turno.Plan = p.Plan;
            }

            return turno;
        }


        public static Turno MapearAEntidadListado(SqlDataReader lector)
        {
            var turno = new Turno();

            if (TieneColumna(lector, "IdTurno"))
                turno.IdTurno = lector.GetInt32(lector.GetOrdinal("IdTurno"));

            if (TieneColumna(lector, "Observaciones"))
                turno.Observaciones = lector.IsDBNull(lector.GetOrdinal("Observaciones"))
                    ? null
                    : lector.GetString(lector.GetOrdinal("Observaciones"));

            if (TieneColumna(lector, "Estado") || TieneColumna(lector, "EstadoTurno"))
                turno.Estado = EnumeracionMapperDatos.MapearEstadoTurno(
                    lector,
                    TieneColumna(lector, "EstadoTurno") ? "EstadoTurno" : "Estado"
                );

            DateTime fi = DateTime.MinValue, ff = DateTime.MinValue;

            if (TieneColumna(lector, "FechaInicio"))
                fi = lector.GetDateTime(lector.GetOrdinal("FechaInicio"));

            if (TieneColumna(lector, "FechaFin"))
                ff = lector.GetDateTime(lector.GetOrdinal("FechaFin"));

            if (fi != DateTime.MinValue && ff != DateTime.MinValue)
                turno.Horario = new HorarioTurno(fi, ff, validar: false);

            if (TieneColumna(lector, "IdPaciente"))
            {
                turno.Paciente = new Paciente
                {
                    IdPaciente = lector.GetInt32(lector.GetOrdinal("IdPaciente")),
                    Nombre = TieneColumna(lector, "NombrePaciente")
                        ? lector.GetString(lector.GetOrdinal("NombrePaciente"))
                        : null,
                    Dni = TieneColumna(lector, "NumeroDocumentoPaciente")
                        ? new DocumentoIdentidad(lector.GetString(lector.GetOrdinal("NumeroDocumentoPaciente")))
                        : null,
                    Apellido = TieneColumna(lector, "ApellidoPaciente")
                        ? lector.GetString(lector.GetOrdinal("ApellidoPaciente"))
                        : null
                };
            }

            if (TieneColumna(lector, "IdMedico"))
            {
                turno.Medico = new Medico
                {
                    IdMedico = lector.GetInt32(lector.GetOrdinal("IdMedico")),
                    Nombre = TieneColumna(lector, "NombreMedico")
                        ? lector.GetString(lector.GetOrdinal("NombreMedico"))
                        : null,
                    Apellido = TieneColumna(lector, "ApellidoMedico")
                        ? lector.GetString(lector.GetOrdinal("ApellidoMedico"))
                        : null,
                    Matricula = TieneColumna(lector, "Matricula")
                        ? lector.GetString(lector.GetOrdinal("Matricula"))
                        : null
                };
            }

            if (TieneColumna(lector, "IdEspecialidad"))
            {
                turno.Especialidad = new Especialidad
                {
                    IdEspecialidad = lector.GetInt32(lector.GetOrdinal("IdEspecialidad")),
                    Nombre = TieneColumna(lector, "NombreEspecialidad")
                        ? lector.GetString(lector.GetOrdinal("NombreEspecialidad"))
                        : null
                };
            }

            if (TieneColumna(lector, "IdCobertura"))
            {
                turno.Cobertura = new Cobertura
                {
                    IdCobertura = lector.GetInt32(lector.GetOrdinal("IdCobertura")),
                    Nombre = TieneColumna(lector, "NombreCobertura")
                        ? lector.GetString(lector.GetOrdinal("NombreCobertura"))
                        : null
                };
            }

            if (TieneColumna(lector, "IdPlan") && !lector.IsDBNull(lector.GetOrdinal("IdPlan")))
            {
                turno.Plan = new Plan
                {
                    IdPlan = lector.GetInt32(lector.GetOrdinal("IdPlan")),
                    Nombre = TieneColumna(lector, "NombrePlan")
                        ? lector.GetString(lector.GetOrdinal("NombrePlan"))
                        : null
                };
            }
            return turno;
        }


        private static bool TieneColumna(SqlDataReader lector, string nombreColumna)
        {
            for (int i = 0; i < lector.FieldCount; i++)
            {
                if (lector.GetName(i).Equals(nombreColumna, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

    }
}
