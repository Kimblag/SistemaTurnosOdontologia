using SGTO.Datos.Infraestructura;
using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SGTO.Datos.Repositorios
{
    public class TurnoRepositorio
    {
        public bool ExisteTurnoActivoPorCobertura(int idCobertura)
        {
            bool resultado = false;

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"SELECT COUNT(*)
                                FROM Turno
                            WHERE IdCobertura = @IdCobertura
                                    AND Estado NOT IN ('C', 'Z', 'X')";
                datos.LimpiarParametros();
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@IdCobertura", idCobertura);

                using (SqlDataReader lector = datos.EjecutarConsulta())
                {
                    try
                    {
                        if (lector.Read())
                        {
                            int cantidad = lector.GetInt32(0);
                            resultado = cantidad > 0;
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return resultado;
        }

        public bool ExisteTurnoActivoPorPlan(int idPlan)
        {
            bool resultado = false;

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"SELECT COUNT(*)
                                    FROM Turno
                                WHERE IdPlan = @IdPlan
                                    AND Estado NOT IN ('C', 'Z', 'X')";
                datos.LimpiarParametros();
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@IdPlan", idPlan);

                using (SqlDataReader lector = datos.EjecutarConsulta())
                {
                    try
                    {
                        if (lector.Read())
                        {
                            int cantidad = lector.GetInt32(0);
                            resultado = cantidad > 0;
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return resultado;
        }

        public bool ExisteTurnoActivoPorPaciente(int idPaciente)
        {
            bool resultado = false;
            string query = @"SELECT COUNT(*)
                         FROM Turno
                         WHERE IdPaciente = @IdPaciente
                           AND Estado NOT IN ('C', 'Z', 'X')";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                datos.LimpiarParametros();
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@IdPaciente", idPaciente);

                try
                {
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            int cantidad = lector.GetInt32(0);
                            resultado = cantidad > 0;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return resultado;
        }


        public List<Turno> ListaPorPaciente(int idPaciente)
        {
            List<Turno> turnos = new List<Turno>();

            string query = @"
                            SELECT 
                                T.IdTurno,
                                T.FechaInicio,
                                T.FechaFin,
                                T.Estado AS EstadoTurno,
                                T.Observaciones,
                                M.IdMedico,
                                M.Nombre AS NombreMedico,
                                M.Apellido AS ApellidoMedico,
                                E.IdEspecialidad,
                                E.Nombre AS NombreEspecialidad,
                                TR.IdTratamiento,
                                TR.Nombre AS NombreTratamiento
                            FROM Turno T
                                INNER JOIN Medico M ON T.IdMedico = M.IdMedico
                                INNER JOIN Especialidad E ON T.IdEspecialidad = E.IdEspecialidad
                                INNER JOIN Tratamiento TR ON T.IdTratamiento = TR.IdTratamiento
                            WHERE T.IdPaciente = @IdPaciente
                            ORDER BY T.FechaInicio DESC";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                datos.LimpiarParametros();
                datos.EstablecerParametros("@IdPaciente", idPaciente);
                datos.DefinirConsulta(query);
                try
                {
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        while (lector.Read())
                        {
                            Turno turno = TurnoMapper.MapearAEntidadBasico(lector);
                            turnos.Add(turno);
                        }
                    }
                    return turnos;
                }
                catch (Exception)
                {
                    throw;
                }

            }

        }


    }
}
