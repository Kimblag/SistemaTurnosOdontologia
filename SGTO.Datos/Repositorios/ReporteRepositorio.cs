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


        public ReporteMedicosKpiDto ConsultarKpisMedicos(DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            try
            {
                using (ConexionDBFactory datos = new ConexionDBFactory())
                {
                    string query = @"
                SELECT
                    (SELECT COUNT(*) FROM Medico) AS TotalMedicos,
                    (SELECT COUNT(*) FROM Medico WHERE Estado = 'A') AS Activos,
                    (SELECT COUNT(*) FROM Turno T 
                        WHERE (@Desde IS NULL OR T.FechaInicio >= @Desde)
                        AND (@Hasta IS NULL OR T.FechaInicio <= @Hasta)
                    ) AS TotalTurnosRealizados,
                    (SELECT COUNT(DISTINCT IdEspecialidad) FROM Especialidad WHERE Estado = 'A') AS EspecialidadesCubiertas";

                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@Desde", fechaDesde ?? (object)DBNull.Value);
                    datos.EstablecerParametros("@Hasta", fechaHasta ?? (object)DBNull.Value);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            return new ReporteMedicosKpiDto
                            {
                                TotalMedicos = lector.GetInt32(lector.GetOrdinal("TotalMedicos")),
                                Activos = lector.GetInt32(lector.GetOrdinal("Activos")),
                                TotalTurnosRealizados = lector.GetInt32(lector.GetOrdinal("TotalTurnosRealizados")),
                                EspecialidadesCubiertas = lector.GetInt32(lector.GetOrdinal("EspecialidadesCubiertas")),
                                ConMasPacientes = 0 // Lógica compleja omitida para simplicidad
                            };
                        }
                    }
                    return new ReporteMedicosKpiDto();
                }
            }
            catch (Exception) { throw; }
        }

        public List<ReporteMedicosDto> ConsultarMedicosFiltrado(DateTime? fechaDesde, DateTime? fechaHasta, int? idEspecialidad)
        {
            List<ReporteMedicosDto> lista = new List<ReporteMedicosDto>();
            try
            {
                using (ConexionDBFactory datos = new ConexionDBFactory())
                {
                    string query = @"
                SELECT 
                    M.IdMedico,
                    M.Apellido + ', ' + M.Nombre AS NombreCompleto,
                    M.Matricula,
                    
                    -- Subconsulta para concatenar especialidades
                    ISNULL(STUFF((
                        SELECT ', ' + E.Nombre
                        FROM MedicoEspecialidad ME
                        INNER JOIN Especialidad E ON ME.IdEspecialidad = E.IdEspecialidad
                        WHERE ME.IdMedico = M.IdMedico
                        FOR XML PATH('')
                    ), 1, 2, ''), 'Sin especialidad') AS Especialidad,

                    M.Estado,
                    COUNT(T.IdTurno) AS TotalTurnos,
                    COUNT(DISTINCT T.IdPaciente) AS PacientesAtendidos,
                    MAX(T.FechaInicio) AS UltimoTurno

                FROM Medico M
                LEFT JOIN Turno T ON M.IdMedico = T.IdMedico 
                
                WHERE 
                    -- Filtro de fechas (Ahora está en el WHERE para filtrar las FILAS resultantes)
                    (@Desde IS NULL OR T.FechaInicio >= @Desde)
                    AND (@Hasta IS NULL OR T.FechaInicio <= @Hasta)
                    
                    -- Filtro por especialidad
                    AND (@Especialidad IS NULL OR EXISTS (
                        SELECT 1 FROM MedicoEspecialidad MEFilter 
                        WHERE MEFilter.IdMedico = M.IdMedico 
                        AND MEFilter.IdEspecialidad = @Especialidad
                    ))

                GROUP BY M.IdMedico, M.Apellido, M.Nombre, M.Matricula, M.Estado
                ORDER BY M.Apellido, M.Nombre";

                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@Desde", fechaDesde ?? (object)DBNull.Value);
                    datos.EstablecerParametros("@Hasta", fechaHasta ?? (object)DBNull.Value);
                    datos.EstablecerParametros("@Especialidad", idEspecialidad ?? (object)DBNull.Value);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new ReporteMedicosDto
                            {
                                IdMedico = lector.GetInt32(lector.GetOrdinal("IdMedico")),
                                NombreCompleto = lector.GetString(lector.GetOrdinal("NombreCompleto")),
                                Matricula = lector.GetString(lector.GetOrdinal("Matricula")),
                                Especialidad = lector.GetString(lector.GetOrdinal("Especialidad")),
                                Estado = lector.GetString(lector.GetOrdinal("Estado")) == "A" ? "Activo" : "Inactivo",
                                TotalTurnos = lector.GetInt32(lector.GetOrdinal("TotalTurnos")),
                                PacientesAtendidos = lector.GetInt32(lector.GetOrdinal("PacientesAtendidos")),
                                UltimoTurno = lector.IsDBNull(lector.GetOrdinal("UltimoTurno")) ? (DateTime?)null : lector.GetDateTime(lector.GetOrdinal("UltimoTurno"))
                            });
                        }
                    }
                }
                return lista;
            }
            catch (Exception) { throw; }
        }

        public List<ReporteTratamientosDto> ConsultarTratamientosFiltrado(DateTime? fechaDesde, DateTime? fechaHasta, int? idEspecialidad)
        {
            List<ReporteTratamientosDto> lista = new List<ReporteTratamientosDto>();
            try
            {
                using (ConexionDBFactory datos = new ConexionDBFactory())
                {
                    // Consulta: Une Tratamientos con sus ejecuciones en Historia Clínica
                    string query = @"
                SELECT 
                    T.IdTratamiento,
                    T.Nombre,
                    E.Nombre AS Especialidad,
                    T.CostoBase,
                    T.Estado,
                    -- Contamos cuántas veces aparece este tratamiento en historias clínicas dentro del rango
                    COUNT(HCR.IdHistoriaClinicaRegistro) AS CantidadRealizados,
                    -- Calculamos ingreso estimado
                    (COUNT(HCR.IdHistoriaClinicaRegistro) * T.CostoBase) AS IngresosEstimados
                FROM Tratamiento T
                INNER JOIN Especialidad E ON T.IdEspecialidad = E.IdEspecialidad
                LEFT JOIN HistoriaClinicaRegistro HCR ON T.IdTratamiento = HCR.IdTratamiento 
                    AND (@Desde IS NULL OR HCR.FechaAtencion >= @Desde)
                    AND (@Hasta IS NULL OR HCR.FechaAtencion <= @Hasta)
                WHERE 
                    (@Especialidad IS NULL OR T.IdEspecialidad = @Especialidad)
                GROUP BY 
                    T.IdTratamiento, T.Nombre, E.Nombre, T.CostoBase, T.Estado
                ORDER BY 
                    CantidadRealizados DESC, T.Nombre ASC";

                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@Desde", fechaDesde ?? (object)DBNull.Value);
                    datos.EstablecerParametros("@Hasta", fechaHasta ?? (object)DBNull.Value);
                    datos.EstablecerParametros("@Especialidad", idEspecialidad ?? (object)DBNull.Value);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new ReporteTratamientosDto
                            {
                                IdTratamiento = lector.GetInt32(lector.GetOrdinal("IdTratamiento")),
                                Nombre = lector.GetString(lector.GetOrdinal("Nombre")),
                                Especialidad = lector.GetString(lector.GetOrdinal("Especialidad")),
                                CostoBase = lector.GetDecimal(lector.GetOrdinal("CostoBase")),
                                Estado = lector.GetString(lector.GetOrdinal("Estado")) == "A" ? "Activo" : "Inactivo",
                                CantidadRealizados = lector.GetInt32(lector.GetOrdinal("CantidadRealizados")),
                                IngresosEstimados = lector.GetDecimal(lector.GetOrdinal("IngresosEstimados"))
                            });
                        }
                    }
                }
                return lista;
            }
            catch (Exception) { throw; }
        }

        public ReporteTratamientosKpiDto ConsultarKpisTratamientos(DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            try
            {
                using (ConexionDBFactory datos = new ConexionDBFactory())
                {
                    string query = @"
                SELECT
                    (SELECT COUNT(*) FROM Tratamiento WHERE Estado = 'A') AS TotalEnCatalogo,
                    
                    (SELECT COUNT(*) FROM HistoriaClinicaRegistro HCR
                        WHERE (@Desde IS NULL OR HCR.FechaAtencion >= @Desde)
                        AND (@Hasta IS NULL OR HCR.FechaAtencion <= @Hasta)
                        AND IdTratamiento IS NOT NULL
                    ) AS TotalRealizados,

                    (SELECT ISNULL(SUM(T.CostoBase), 0) 
                        FROM HistoriaClinicaRegistro HCR
                        INNER JOIN Tratamiento T ON HCR.IdTratamiento = T.IdTratamiento
                        WHERE (@Desde IS NULL OR HCR.FechaAtencion >= @Desde)
                        AND (@Hasta IS NULL OR HCR.FechaAtencion <= @Hasta)
                    ) AS IngresoTotalEstimado";

                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@Desde", fechaDesde ?? (object)DBNull.Value);
                    datos.EstablecerParametros("@Hasta", fechaHasta ?? (object)DBNull.Value);

                    var dto = new ReporteTratamientosKpiDto();

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            dto.TotalEnCatalogo = lector.GetInt32(lector.GetOrdinal("TotalEnCatalogo"));
                            dto.TotalRealizados = lector.GetInt32(lector.GetOrdinal("TotalRealizados"));
                            dto.IngresoTotalEstimado = lector.GetDecimal(lector.GetOrdinal("IngresoTotalEstimado"));
                        }
                    }

                    dto.TratamientoMasSolicitado = "-";
                    dto.EspecialidadMasDemandada = "-";

                    return dto;
                }
            }
            catch (Exception) { throw; }
        }

    }
}
