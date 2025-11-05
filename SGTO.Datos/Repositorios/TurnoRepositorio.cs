using SGTO.Datos.Infraestructura;
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
    }
}
