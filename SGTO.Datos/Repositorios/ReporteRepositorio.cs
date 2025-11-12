using SGTO.Comun.DTOs;
using SGTO.Datos.Infraestructura;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SGTO.Datos.Repositorios
{
    public class ReporteRepositorio
    {

        public List<ReportePacientesDto> ConsultarPacientes()
        {
            List<ReportePacientesDto> lista = new List<ReportePacientesDto>();

            try
            {
                using (ConexionDBFactory datos = new ConexionDBFactory())
                {
                    string query = @"
                        SELECT 
                            P.IdPaciente,
                            P.Apellido + ', ' + P.Nombre AS NombreCompleto,
                            P.NumeroDocumento AS NumeroDocumento,
                            C.Nombre AS Cobertura,
                            PL.Nombre AS [Plan],
                            COUNT(T.IdTurno) AS TotalTurnos,
                            MAX(T.FechaInicio) AS UltimaAtencion,
                            (
                                SELECT TOP 1 (M.Apellido + ', ' + M.Nombre)
                                FROM Turno T2
                                INNER JOIN Medico M ON T2.IdMedico = M.IdMedico
                                WHERE T2.IdPaciente = P.IdPaciente
                                GROUP BY M.Apellido, M.Nombre
                                ORDER BY COUNT(*) DESC
                            ) AS MedicoFrecuente
                        FROM Paciente P
                        LEFT JOIN Cobertura C ON P.IdCobertura = C.IdCobertura
                        LEFT JOIN [Plan] PL ON P.IdPlan = PL.IdPlan
                        LEFT JOIN Turno T ON P.IdPaciente = T.IdPaciente
                        WHERE P.Estado = 'A'
                        GROUP BY 
                            P.IdPaciente, P.Apellido, P.Nombre, P.NumeroDocumento, C.Nombre, PL.Nombre
                        ORDER BY 
                            P.Apellido, P.Nombre";

                    datos.DefinirConsulta(query);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        int ordIdPaciente = lector.GetOrdinal("IdPaciente");
                        int ordNombreCompleto = lector.GetOrdinal("NombreCompleto");
                        int ordNumeroDocumento = lector.GetOrdinal("NumeroDocumento");
                        int ordCobertura = lector.GetOrdinal("Cobertura");
                        int ordPlan = lector.GetOrdinal("Plan");
                        int ordTotalTurnos = lector.GetOrdinal("TotalTurnos");
                        int ordUltimaAtencion = lector.GetOrdinal("UltimaAtencion");
                        int ordMedicoFrecuente = lector.GetOrdinal("MedicoFrecuente");

                        while (lector.Read())
                        {
                            var dto = new ReportePacientesDto
                            {
                                IdPaciente = lector.GetInt32(ordIdPaciente),
                                NombreCompleto = lector.IsDBNull(ordNombreCompleto) ? string.Empty : lector.GetString(ordNombreCompleto),
                                NumeroDocumento = lector.IsDBNull(ordNumeroDocumento) ? string.Empty : lector.GetString(ordNumeroDocumento),
                                Cobertura = lector.IsDBNull(ordCobertura) ? "Sin cobertura" : lector.GetString(ordCobertura),
                                Plan = lector.IsDBNull(ordPlan) ? "-" : lector.GetString(ordPlan),
                                TotalTurnos = lector.IsDBNull(ordTotalTurnos) ? 0 : lector.GetInt32(ordTotalTurnos),
                                UltimaAtencion = lector.IsDBNull(ordUltimaAtencion) ? (DateTime?)null : lector.GetDateTime(ordUltimaAtencion),
                                MedicoFrecuente = lector.IsDBNull(ordMedicoFrecuente) ? "-" : lector.GetString(ordMedicoFrecuente)
                            };

                            lista.Add(dto);
                        }
                    }
                }

                return lista;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ReportePacientesKpiDto ConsultarKpisPacientes(DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            try
            {
                using (ConexionDBFactory datos = new ConexionDBFactory())
                {
                    string query = @"
                        SELECT
                            (SELECT COUNT(*) FROM Paciente WHERE Estado = 'A') AS TotalPacientes,
                            (SELECT COUNT(DISTINCT P.IdPaciente)
                                FROM Paciente P
                                INNER JOIN Turno T ON P.IdPaciente = T.IdPaciente
                                WHERE T.Estado IN ('Z','X')) AS Atendidos,
                            (SELECT COUNT(*) FROM Paciente 
                                WHERE Estado = 'A' 
                                AND (@Desde IS NULL OR FechaAlta >= @Desde)
                                AND (@Hasta IS NULL OR FechaAlta <= @Hasta)) AS NuevosEnPeriodo,
                            (SELECT COUNT(*) FROM Paciente WHERE Estado = 'A' AND IdCobertura <> 1) AS ConCobertura,
                            (SELECT COUNT(*) FROM Paciente WHERE Estado = 'A' AND IdCobertura = 1) AS Particulares";

                    datos.DefinirConsulta(query);

                    datos.EstablecerParametros("@Desde", fechaDesde ?? (object)DBNull.Value);
                    datos.EstablecerParametros("@Hasta", fechaHasta ?? (object)DBNull.Value);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            return new ReportePacientesKpiDto
                            {
                                TotalPacientes = lector.GetInt32(lector.GetOrdinal("TotalPacientes")),
                                Atendidos = lector.GetInt32(lector.GetOrdinal("Atendidos")),
                                NuevosEnPeriodo = lector.GetInt32(lector.GetOrdinal("NuevosEnPeriodo")),
                                ConCobertura = lector.GetInt32(lector.GetOrdinal("ConCobertura")),
                                Particulares = lector.GetInt32(lector.GetOrdinal("Particulares"))
                            };
                        }
                    }

                    return new ReportePacientesKpiDto();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ReportePacientesDto> ConsultarPacientesFiltrado(DateTime? fechaDesde, DateTime? fechaHasta, int? idCobertura, int? idPlan)
        {
            List<ReportePacientesDto> lista = new List<ReportePacientesDto>();

            try
            {
                using (ConexionDBFactory datos = new ConexionDBFactory())
                {
                    string query = @"
                SELECT 
                    P.IdPaciente,
                    P.Apellido + ', ' + P.Nombre AS NombreCompleto,
                    P.NumeroDocumento AS NumeroDocumento,
                    C.Nombre AS Cobertura,
                    PL.Nombre AS [Plan],
                    COUNT(T.IdTurno) AS TotalTurnos,
                    MAX(T.FechaInicio) AS UltimaAtencion,
                    (
                        SELECT TOP 1 (M.Apellido + ', ' + M.Nombre)
                        FROM Turno T2
                        INNER JOIN Medico M ON T2.IdMedico = M.IdMedico
                        WHERE T2.IdPaciente = P.IdPaciente
                        GROUP BY M.Apellido, M.Nombre
                        ORDER BY COUNT(*) DESC
                    ) AS MedicoFrecuente
                FROM Paciente P
                LEFT JOIN Cobertura C ON P.IdCobertura = C.IdCobertura
                LEFT JOIN [Plan] PL ON P.IdPlan = PL.IdPlan
                LEFT JOIN Turno T ON P.IdPaciente = T.IdPaciente
                WHERE P.Estado = 'A'
                    AND (@Desde IS NULL OR T.FechaInicio >= @Desde)
                    AND (@Hasta IS NULL OR T.FechaInicio <= @Hasta)
                    AND (@Cobertura IS NULL OR P.IdCobertura = @Cobertura)
                    AND (@Plan IS NULL OR P.IdPlan = @Plan)
                GROUP BY 
                    P.IdPaciente, P.Apellido, P.Nombre, P.NumeroDocumento, C.Nombre, PL.Nombre
                ORDER BY 
                    P.Apellido, P.Nombre";

                    datos.DefinirConsulta(query);

                    datos.EstablecerParametros("@Desde", fechaDesde ?? (object)DBNull.Value);
                    datos.EstablecerParametros("@Hasta", fechaHasta ?? (object)DBNull.Value);
                    datos.EstablecerParametros("@Cobertura", idCobertura ?? (object)DBNull.Value);
                    datos.EstablecerParametros("@Plan", idPlan ?? (object)DBNull.Value);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        int ordIdPaciente = lector.GetOrdinal("IdPaciente");
                        int ordNombreCompleto = lector.GetOrdinal("NombreCompleto");
                        int ordNumeroDocumento = lector.GetOrdinal("NumeroDocumento");
                        int ordCobertura = lector.GetOrdinal("Cobertura");
                        int ordPlan = lector.GetOrdinal("Plan");
                        int ordTotalTurnos = lector.GetOrdinal("TotalTurnos");
                        int ordUltimaAtencion = lector.GetOrdinal("UltimaAtencion");
                        int ordMedicoFrecuente = lector.GetOrdinal("MedicoFrecuente");

                        while (lector.Read())
                        {
                            var dto = new ReportePacientesDto
                            {
                                IdPaciente = lector.GetInt32(ordIdPaciente),
                                NombreCompleto = lector.IsDBNull(ordNombreCompleto) ? string.Empty : lector.GetString(ordNombreCompleto),
                                NumeroDocumento = lector.IsDBNull(ordNumeroDocumento) ? string.Empty : lector.GetString(ordNumeroDocumento),
                                Cobertura = lector.IsDBNull(ordCobertura) ? "Sin cobertura" : lector.GetString(ordCobertura),
                                Plan = lector.IsDBNull(ordPlan) ? "-" : lector.GetString(ordPlan),
                                TotalTurnos = lector.IsDBNull(ordTotalTurnos) ? 0 : lector.GetInt32(ordTotalTurnos),
                                UltimaAtencion = lector.IsDBNull(ordUltimaAtencion) ? (DateTime?)null : lector.GetDateTime(ordUltimaAtencion),
                                MedicoFrecuente = lector.IsDBNull(ordMedicoFrecuente) ? "-" : lector.GetString(ordMedicoFrecuente)
                            };

                            lista.Add(dto);
                        }
                    }
                }

                return lista;
            }
            catch (Exception)
            {
                throw;
            }
        }





    }
}
