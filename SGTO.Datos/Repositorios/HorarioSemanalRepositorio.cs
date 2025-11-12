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
                datos.DefinirProcedimiento("sp_GenerarAgendaMedica");
                datos.EstablecerParametros("@DiasAdelante", 60);
                datos.EstablecerParametros("@IdMedico", idMedico);
                datos.EstablecerParametros("@IncluirMedicosInactivos", 0);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al ejecutar sp_GenerarAgendaMedica: " + ex.Message);
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



    }
}
