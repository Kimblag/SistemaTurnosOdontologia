using SGTO.Datos.Infraestructura;
using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SGTO.Datos.Repositorios
{
    public class CoberturaRepositorio
    {
        public List<Cobertura> Listar(string estado = null)
        {
            List<Cobertura> coberturas = new List<Cobertura>();


            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"SELECT C.IdCobertura, 
	                                       C.Nombre AS NombreCobertura, 
	                                       C.Descripcion AS DescripcionCobertura,
	                                       C.Estado AS EstadoCobertura,
	                                       P.IdPlan,
	                                       P.Nombre AS NombrePlan,
	                                       P.Descripcion AS DescripcionPlan,
	                                       P.PorcentajeCobertura,
	                                       P.Estado AS EstadoPlan
	                                    FROM Cobertura C
	                                    LEFT JOIN [Plan] P ON C.IdCobertura = P.IdCobertura";
                if (estado != null)
                {
                    query += $" WHERE LOWER(C.Estado) = LOWER('{estado.Substring(0, 1)}')";
                }

                try
                {
                    datos.DefinirConsulta(query);

                    // usamos using para aplicar buenas prácticas y evitar conexiones abiertas 
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        int idCoberturaActual = -1;
                        Cobertura coberturaActual = null;

                        while (lector.Read())
                        {
                            int IdCobertura = lector.GetInt32(lector.GetOrdinal("IdCobertura"));

                            if (IdCobertura != idCoberturaActual)
                            {
                                coberturaActual = CoberturaMapper.MapearAEntidad(lector);
                                coberturas.Add(coberturaActual);

                                idCoberturaActual = IdCobertura;
                            }

                            if (!lector.IsDBNull(lector.GetOrdinal("IdPlan")))
                            {
                                Plan plan = PlanMapper.MapearAEntidad(lector, null);
                                coberturaActual.Planes.Add(plan);
                            }
                        }
                    }

                    return coberturas;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        public Cobertura ObtenerCoberturaPorId(int idCobertura)
        {
            Cobertura cobertura = null;

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"SELECT C.IdCobertura,
                                    C.Nombre AS NombreCobertura,
                                    C.Descripcion AS DescripcionCobertura,
                                    C.Estado AS EstadoCobertura
                                FROM Cobertura C
                                WHERE C.IdCobertura = @IdCobertura";
                datos.EstablecerParametros("@IdCobertura", idCobertura);
                datos.DefinirConsulta(query);

                using (SqlDataReader lector = datos.EjecutarConsulta())
                {
                    try
                    {
                        if (lector.Read())
                        {
                            cobertura = CoberturaMapper.MapearAEntidad(lector);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return cobertura;
        }


        public bool Modificar(Cobertura cobertura)
        {
            bool resultado = false;
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"UPDATE Cobertura 
                                SET Nombre = @Nombre, 
                                    Descripcion = @Descripcion,
                                    Estado = @Estado
                            WHERE IdCobertura = @IdCobertura";

                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@Nombre", cobertura.Nombre);
                    datos.EstablecerParametros("@Descripcion", cobertura.Descripcion);
                    datos.EstablecerParametros("@Estado", cobertura.Estado.ToString().ToUpper().Substring(0, 1));
                    datos.EstablecerParametros("@IdCobertura", cobertura.IdCobertura);

                    datos.EjecutarAccion();
                    resultado = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return resultado;
        }


        public void DarDeBaja(int idCobertura, char estado, ConexionDBFactory datos)
        {
            string query = @"UPDATE Cobertura 
                                    SET Estado = @Estado
                                WHERE IdCobertura = @IdCobertura";

            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@Estado", estado);
            datos.EstablecerParametros("@IdCobertura", idCobertura);
            datos.EjecutarAccion();
        }


    }
}
