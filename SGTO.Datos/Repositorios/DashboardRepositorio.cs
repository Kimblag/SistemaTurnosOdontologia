using SGTO.Comun.DTOs;
using SGTO.Datos.Infraestructura;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SGTO.Datos.Repositorios
{
    public class DashboardRepositorio
    {
        public DashboardResumenDto ObtenerResumenDiario()
        {
            string query = @"
                SELECT 
                    (SELECT COUNT(*) FROM Turno WHERE CONVERT(date, FechaInicio) = CONVERT(date, GETDATE())) AS TurnosDelDia,
                    (SELECT COUNT(*) FROM HistoriaClinicaRegistro WHERE CONVERT(date, FechaAtencion) = CONVERT(date, GETDATE())) AS PacientesAtendidos,
                    (SELECT COUNT(*) FROM Turno WHERE Estado = 'R' AND CONVERT(date, FechaInicio) = CONVERT(date, GETDATE())) AS Reprogramados,
                    (SELECT COUNT(*) FROM Turno WHERE Estado = 'C' AND CONVERT(date, FechaInicio) = CONVERT(date, GETDATE())) AS Cancelados;
            ";

            DashboardResumenDto resumen = new DashboardResumenDto();

            try
            {
                using (ConexionDBFactory datos = new ConexionDBFactory())
                {
                    datos.DefinirConsulta(query);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            resumen.TurnosDelDia = lector.GetInt32(0);
                            resumen.PacientesAtendidos = lector.GetInt32(1);
                            resumen.Reprogramados = lector.GetInt32(2);
                            resumen.Cancelados = lector.GetInt32(3);
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

        public List<DashboardActividadSemanalDto> ObtenerActividadSemanal()
        {
            string query = @"
                        SELECT 
                            d.Dia,
                            ISNULL(x.Cantidad, 0) AS Cantidad
                        FROM (
                            SELECT 1 AS NumDia, N'Lunes' AS Dia
                            UNION ALL SELECT 2, N'Martes'
                            UNION ALL SELECT 3, N'Miércoles'
                            UNION ALL SELECT 4, N'Jueves'
                            UNION ALL SELECT 5, N'Viernes'
                            UNION ALL SELECT 6, N'Sábado'
                            UNION ALL SELECT 7, N'Domingo'
                        ) AS d
                        LEFT JOIN (
                            SELECT 
                                ((DATEPART(WEEKDAY, t.FechaInicio) + @@DATEFIRST - 2) % 7) + 1 AS NumDia,
                                COUNT(*) AS Cantidad
                            FROM Turno t
                            WHERE 
                                CONVERT(date, t.FechaInicio) >= DATEADD(DAY, -(((DATEPART(WEEKDAY, GETDATE()) + @@DATEFIRST - 2) % 7)), CONVERT(date, GETDATE()))
                                AND CONVERT(date, t.FechaInicio) <  DATEADD(DAY, 7 - (((DATEPART(WEEKDAY, GETDATE()) + @@DATEFIRST - 2) % 7)), CONVERT(date, GETDATE()))
                            GROUP BY ((DATEPART(WEEKDAY, t.FechaInicio) + @@DATEFIRST - 2) % 7) + 1
                        ) AS x ON d.NumDia = x.NumDia
                        ORDER BY d.NumDia;";

            List<DashboardActividadSemanalDto> lista = new List<DashboardActividadSemanalDto>();

            try
            {
                using (ConexionDBFactory datos = new ConexionDBFactory())
                {
                    datos.DefinirConsulta(query);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        while (lector.Read())
                        {
                            DashboardActividadSemanalDto dto = new DashboardActividadSemanalDto
                            {
                                Dia = lector.GetString(0),
                                Cantidad = lector.GetInt32(1)
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
