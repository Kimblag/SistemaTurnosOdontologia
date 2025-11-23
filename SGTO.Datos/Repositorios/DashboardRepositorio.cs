using SGTO.Comun.DTOs;
using SGTO.Datos.Infraestructura;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SGTO.Datos.Repositorios
{
    public class DashboardRepositorio
    {
        public DashboardResumenDto ObtenerResumenDiario(int? idMedico)
        {
            string query = @"
                SELECT 
                    (SELECT COUNT(*) FROM Turno 
                     WHERE CONVERT(date, FechaInicio) = CONVERT(date, GETDATE())
                     AND (@IdMedico IS NULL OR IdMedico = @IdMedico)) AS TurnosDelDia,

                    (SELECT COUNT(*) FROM HistoriaClinicaRegistro 
                     WHERE CONVERT(date, FechaAtencion) = CONVERT(date, GETDATE())
                     AND (@IdMedico IS NULL OR IdMedico = @IdMedico)) AS PacientesAtendidos,

                    (SELECT COUNT(*) FROM Turno 
                     WHERE Estado = 'R' AND CONVERT(date, FechaInicio) = CONVERT(date, GETDATE())
                     AND (@IdMedico IS NULL OR IdMedico = @IdMedico)) AS Reprogramados,

                    (SELECT COUNT(*) FROM Turno 
                     WHERE Estado = 'C' AND CONVERT(date, FechaInicio) = CONVERT(date, GETDATE())
                     AND (@IdMedico IS NULL OR IdMedico = @IdMedico)) AS Cancelados
            ";

            DashboardResumenDto resumen = new DashboardResumenDto();

            try
            {
                using (ConexionDBFactory datos = new ConexionDBFactory())
                {
                    datos.DefinirConsulta(query);

                    object paramValue = idMedico.HasValue ? (object)idMedico.Value : DBNull.Value;
                    datos.EstablecerParametros("@IdMedico", paramValue);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            resumen.TurnosDelDia = lector.IsDBNull(0) ? 0 : lector.GetInt32(0);
                            resumen.PacientesAtendidos = lector.IsDBNull(1) ? 0 : lector.GetInt32(1);
                            resumen.Reprogramados = lector.IsDBNull(2) ? 0 : lector.GetInt32(2);
                            resumen.Cancelados = lector.IsDBNull(3) ? 0 : lector.GetInt32(3);
                        }
                    }
                }
                return resumen;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DashboardActividadSemanalDto> ObtenerActividadSemanal(int? idMedico)
        {
            string query = @"
                        DECLARE @Hoy DATE = CAST(GETDATE() AS DATE);
                        DECLARE @InicioSemana DATE = DATEADD(week, DATEDIFF(week, 0, DATEADD(day, -1, @Hoy)), 0);
                        IF (@Hoy = DATEADD(day, 6, @InicioSemana))
                        BEGIN
                            SET @InicioSemana = DATEADD(day, 7, @InicioSemana);
                        END
                        SELECT 
                            d.DiaNombre,
                            d.FechaDia,
                            ISNULL(COUNT(t.IdTurno), 0) AS Cantidad
                        FROM (
                            SELECT 1 AS Orden, @InicioSemana AS FechaDia, 'Lunes' AS DiaNombre
                            UNION ALL SELECT 2, DATEADD(day, 1, @InicioSemana), 'Martes'
                            UNION ALL SELECT 3, DATEADD(day, 2, @InicioSemana), 'Miércoles'
                            UNION ALL SELECT 4, DATEADD(day, 3, @InicioSemana), 'Jueves'
                            UNION ALL SELECT 5, DATEADD(day, 4, @InicioSemana), 'Viernes'
                            UNION ALL SELECT 6, DATEADD(day, 5, @InicioSemana), 'Sábado'
                            UNION ALL SELECT 7, DATEADD(day, 6, @InicioSemana), 'Domingo'
                        ) AS d
                        LEFT JOIN Turno t ON 
                            CAST(t.FechaInicio AS DATE) = d.FechaDia
                            AND t.Estado NOT IN ('C', 'X')
                            AND (@IdMedico IS NULL OR t.IdMedico = @IdMedico)
                        GROUP BY d.Orden, d.DiaNombre, d.FechaDia
                        ORDER BY d.Orden";

            List<DashboardActividadSemanalDto> lista = new List<DashboardActividadSemanalDto>();

            try
            {
                using (ConexionDBFactory datos = new ConexionDBFactory())
                {
                    datos.DefinirConsulta(query);

                    object paramValue = idMedico.HasValue ? (object)idMedico.Value : DBNull.Value;
                    datos.EstablecerParametros("@IdMedico", paramValue);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        while (lector.Read())
                        {
                            DashboardActividadSemanalDto dto = new DashboardActividadSemanalDto
                            {
                                Dia = lector.GetString(0),
                                Fecha = lector.GetDateTime(1),
                                Cantidad = lector.GetInt32(2)
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
