using SGTO.Datos.Infraestructura;
using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace SGTO.Datos.Repositorios
{
    public class HorarioSemanalRepositorio
    {

        public void Crear(List<HorarioSemanalMedico> horarios, ConexionDBFactory datos)
        {
            if (horarios == null || horarios.Count == 0)
                return;

            string query = @"
                    INSERT INTO HorarioSemanalMedico 
                        (IdMedico, DiaSemana, HoraInicio, HoraFin, Estado)
                    VALUES
                        (@IdMedico, @DiaSemana, @HoraInicio, @HoraFin, @Estado)";

            try
            {
                foreach (var h in horarios)
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@IdMedico", h.Medico.IdMedico);
                    datos.EstablecerParametros("@DiaSemana", h.DiaSemana);
                    datos.EstablecerParametros("@HoraInicio", h.HoraInicio);
                    datos.EstablecerParametros("@HoraFin", h.HoraFin);
                    datos.EstablecerParametros("@Estado", h.Estado.ToString()[0]);
                    datos.EjecutarAccion();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error en HorarioSemanalRepositorio.Crear: " + ex.Message);
                throw;
            }
        }

        public void GenerarAgendaParaMedico(int idMedico, ConexionDBFactory datos)
        {
            try
            {
                datos.LimpiarParametros();

                // extraer la duracion del turno que viene de parametros
                datos.DefinirConsulta("SELECT TRY_CAST(Valor AS INT) FROM ParametroSistema WHERE Nombre = 'DuracionTurnoMinutos'");
                int duracion = 60;
                using (var lector = datos.EjecutarConsulta())
                {
                    if (lector.Read() && !lector.IsDBNull(0))
                        duracion = lector.GetInt32(0);
                }
                if (duracion <= 0) duracion = 60;

                DateTime fechaDesde = DateTime.Today;
                DateTime fechaHasta = fechaDesde.AddDays(60);

                // buscar los horarios activos del médico
                var horarios = new List<(byte DiaSemana, TimeSpan Inicio, TimeSpan Fin)>();
                datos.DefinirConsulta(@"SELECT DiaSemana, HoraInicio, HoraFin 
                                FROM HorarioSemanalMedico 
                                WHERE IdMedico = @IdMedico AND Estado = 'A'");
                datos.LimpiarParametros();
                datos.EstablecerParametros("@IdMedico", idMedico);

                using (var lector = datos.EjecutarConsulta())
                {
                    while (lector.Read())
                    {
                        horarios.Add(((byte)lector["DiaSemana"], (TimeSpan)lector["HoraInicio"], (TimeSpan)lector["HoraFin"]));
                    }
                }

                foreach (var h in horarios)
                {
                    for (DateTime f = fechaDesde; f <= fechaHasta; f = f.AddDays(1))
                    {
                        // Lunes = 1 ... Domingo = 7
                        int diaSemana = ((int)f.DayOfWeek == 0) ? 7 : (int)f.DayOfWeek;

                        if (diaSemana == h.DiaSemana)
                        {
                            TimeSpan inicio = h.Inicio;
                            while (inicio < h.Fin)
                            {
                                TimeSpan fin = inicio.Add(TimeSpan.FromMinutes(duracion));
                                if (fin <= h.Fin)
                                {
                                    datos.DefinirConsulta(@"
                                IF NOT EXISTS (
                                    SELECT 1 FROM AgendaMedico 
                                    WHERE IdMedico = @IdMedico AND Fecha = @Fecha AND HoraInicio = @HoraInicio
                                )
                                INSERT INTO AgendaMedico (IdMedico, Fecha, HoraInicio, HoraFin, Estado)
                                VALUES (@IdMedico, @Fecha, @HoraInicio, @HoraFin, 'L')");
                                    datos.LimpiarParametros();
                                    datos.EstablecerParametros("@IdMedico", idMedico);
                                    datos.EstablecerParametros("@Fecha", f.Date);
                                    datos.EstablecerParametros("@HoraInicio", inicio);
                                    datos.EstablecerParametros("@HoraFin", fin);
                                    datos.EjecutarAccion();
                                }
                                inicio = inicio.Add(TimeSpan.FromMinutes(duracion));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al generar agenda en código: " + ex.Message);
                throw;
            }
        }


        public void EliminarPorMedico(int idMedico, ConexionDBFactory datos)
        {
            try
            {
                datos.LimpiarParametros();
                datos.DefinirConsulta("DELETE FROM HorarioSemanalMedico WHERE IdMedico = @IdMedico");
                datos.EstablecerParametros("@IdMedico", idMedico);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error en HorarioSemanalRepositorio.EliminarPorMedico: " + ex.Message);
                throw;
            }
        }

        public List<HorarioSemanalMedico> ObtenerPorMedico(int idMedico)
        {
            var lista = new List<HorarioSemanalMedico>();

            using (var datos = new ConexionDBFactory())
            {
                string query = @"
                    SELECT DiaSemana, HoraInicio, HoraFin, Estado
                        FROM HorarioSemanalMedico
                    WHERE IdMedico = @IdMedico AND Estado = 'A'
                    ORDER BY DiaSemana ASC";

                try
                {
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@IdMedico", idMedico);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new HorarioSemanalMedico
                            {
                                Medico = new Medico { IdMedico = idMedico },
                                DiaSemana = lector.GetByte(lector.GetOrdinal("DiaSemana")),
                                HoraInicio = lector.GetTimeSpan(lector.GetOrdinal("HoraInicio")),
                                HoraFin = lector.GetTimeSpan(lector.GetOrdinal("HoraFin")),
                                Estado = EnumeracionMapperDatos.MapearEstadoEntidad(lector, "Estado")
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error en HorarioSemanalRepositorio.ObtenerPorMedico: " + ex.Message);
                    throw;
                }
            }

            return lista;
        }

        public void ActualizarAgendaPorCambioDeHorario(int idMedico, ConexionDBFactory datos)
        {
            try
            {
                DateTime desde = DateTime.Today;

                // marcar los turnos pendientes de reprogramar si hay alguno
                string queryTurnos = @"
                                UPDATE T
                                SET T.Estado = 'P'
                                FROM Turno T
                                WHERE T.Estado IN ('N','R')
                                    AND T.IdTurno IN (
                                        SELECT A.IdTurno
                                        FROM AgendaMedico A
                                        WHERE A.IdMedico = @IdMedico
                                        AND A.Fecha >= @Desde
                                        AND NOT EXISTS (
                                            SELECT 1 FROM HorarioSemanalMedico H
                                            WHERE H.IdMedico = A.IdMedico
                                                AND H.Estado = 'A'
                                                AND H.DiaSemana = DATEPART(WEEKDAY, A.Fecha)
                                                AND A.HoraInicio >= H.HoraInicio
                                                AND A.HoraFin <= H.HoraFin
                                        )
                                    )";

                datos.DefinirConsulta(queryTurnos);
                datos.LimpiarParametros();
                datos.EstablecerParametros("@IdMedico", idMedico);
                datos.EstablecerParametros("@Desde", desde);
                datos.EjecutarAccion();

                // los slots libres que quedan fuera del horario del m[edico se inactivan
                string queryInactivar = @"
                        UPDATE A
                        SET A.Estado = 'I',
                            A.Observacion = 
                                CASE 
                                    WHEN A.Observacion IS NULL OR LTRIM(RTRIM(A.Observacion)) = ''
                                        THEN 'Inactivado por cambio de horario del médico'
                                    ELSE A.Observacion + ' | Inactivado por cambio de horario del médico'
                                END
                        FROM AgendaMedico A
                        WHERE A.IdMedico = @IdMedico
                          AND A.Fecha >= @Desde
                          AND A.Estado = 'L'
                          AND NOT EXISTS (
                                SELECT 1 FROM HorarioSemanalMedico H
                                WHERE H.IdMedico = A.IdMedico
                                  AND H.Estado = 'A'
                                  AND H.DiaSemana = DATEPART(WEEKDAY, A.Fecha)
                                  AND A.HoraInicio >= H.HoraInicio
                                  AND A.HoraFin <= H.HoraFin
                          )";

                datos.DefinirConsulta(queryInactivar);
                datos.LimpiarParametros();
                datos.EstablecerParametros("@IdMedico", idMedico);
                datos.EstablecerParametros("@Desde", desde);
                datos.EjecutarAccion();

                // los slots ocupados que quedan fuera del horario se marcan con observación
                string queryObservacion = @"
                                UPDATE A
                                SET A.Observacion =
                                        CASE 
                                            WHEN A.Observacion IS NULL OR LTRIM(RTRIM(A.Observacion)) = ''
                                                THEN 'Fuera de horario vigente: requiere reprogramación'
                                            ELSE A.Observacion + ' | Fuera de horario vigente: requiere reprogramación'
                                        END
                                FROM AgendaMedico A
                                WHERE A.IdMedico = @IdMedico
                                  AND A.Fecha >= @Desde
                                  AND A.Estado = 'O'
                                  AND NOT EXISTS (
                                        SELECT 1 FROM HorarioSemanalMedico H
                                        WHERE H.IdMedico = A.IdMedico
                                          AND H.Estado = 'A'
                                          AND H.DiaSemana = DATEPART(WEEKDAY, A.Fecha)
                                          AND A.HoraInicio >= H.HoraInicio
                                          AND A.HoraFin <= H.HoraFin
                                  )";

                datos.DefinirConsulta(queryObservacion);
                datos.LimpiarParametros();
                datos.EstablecerParametros("@IdMedico", idMedico);
                datos.EstablecerParametros("@Desde", desde);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al actualizar agenda en código: " + ex.Message);
                throw;
            }
        }



    }
}
