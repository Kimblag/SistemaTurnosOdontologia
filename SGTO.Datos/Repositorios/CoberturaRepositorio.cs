using SGTO.Datos.Infraestructura;
using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;


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
                                           CPH.PorcentajeCobertura AS PorcentajeCoberturaVigente,
	                                       P.IdPlan,
	                                       P.Nombre AS NombrePlan,
	                                       P.Descripcion AS DescripcionPlan,
	                                       P.PorcentajeCobertura,
	                                       P.Estado AS EstadoPlan
	                                    FROM Cobertura C
	                                    LEFT JOIN [Plan] P ON C.IdCobertura = P.IdCobertura
                                        LEFT JOIN CoberturaPorcentajeHistorial CPH 
                                            ON CPH.IdCobertura = C.IdCobertura AND CPH.Estado = 'A'
                                        {{WHERE}}
                                        ORDER BY NombreCobertura ASC";

                query = estado != null
                       ? query.Replace("{{WHERE}}", $" WHERE UPPER(C.Estado) = UPPER('{estado[0]}')")
                       : query.Replace("{{WHERE}}", " ");

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
                catch (Exception ex)
                {
                    Debug.WriteLine("Capa datos" + ex.Message);
                    throw;
                }
            }
        }

        public Cobertura ObtenerPorId(int idCobertura)
        {
            Cobertura cobertura = null;

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"SELECT 
                                    C.IdCobertura,
                                    C.Nombre AS NombreCobertura,
                                    C.Descripcion AS DescripcionCobertura,
                                    C.Estado AS EstadoCobertura,
                                    CPH.PorcentajeCobertura AS PorcentajeCoberturaVigente
                                FROM Cobertura C
                                LEFT JOIN CoberturaPorcentajeHistorial CPH 
                                    ON CPH.IdCobertura = C.IdCobertura 
                                    AND CPH.Estado = 'A'
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

        public void Modificar(Cobertura cobertura, ConexionDBFactory datos)
        {
            string query = @"UPDATE Cobertura 
                                SET Nombre = @Nombre, 
                                    Descripcion = @Descripcion,
                                    Estado = @Estado
                            WHERE IdCobertura = @IdCobertura";

            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@Nombre", cobertura.Nombre);
            datos.EstablecerParametros("@Descripcion", cobertura.Descripcion);
            datos.EstablecerParametros("@Estado", EnumeracionMapper.ObtenerChar(cobertura.Estado));
            datos.EstablecerParametros("@IdCobertura", cobertura.IdCobertura);
            datos.EjecutarAccion();
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


        public bool ExistePorNombre(string nombreCobertura)
        {
            bool resultado = false;
            string query = @"SELECT COUNT(Nombre)
                                FROM Cobertura
                            WHERE UPPER(Nombre) = @Nombre";
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@Nombre", nombreCobertura.ToUpper());

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            int cantidad = lector.GetInt32(0);
                            resultado = cantidad > 0;
                        }
                    }
                    return resultado;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public int Crear(Cobertura nuevaCobertura, ConexionDBFactory datos)
        {
            string query = @"INSERT INTO Cobertura (Nombre, Descripcion, Estado)
                                OUTPUT INSERTED.IdCobertura
                            VALUES (@Nombre, @Descripcion, @Estado)";
            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@Nombre", nuevaCobertura.Nombre);
            datos.EstablecerParametros("@Descripcion", nuevaCobertura.Descripcion);
            datos.EstablecerParametros("@Estado", EnumeracionMapper.ObtenerChar(nuevaCobertura.Estado));
            int idNuevaCobertura = datos.EjecutarAccionEscalar();

            return idNuevaCobertura;
        }


        public bool EstaDadoDeBaja(int idCobertura)
        {
            bool estaDadoDeBaja = false;
            string query = @"SELECT Estado
                                FROM Cobertura
                            WHERE IdCobertura = @IdCobertura";
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@IdCobertura", idCobertura);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            if (!lector.IsDBNull(lector.GetOrdinal("Estado")))
                            {
                                EstadoEntidad estado = EnumeracionMapper.MapearEstadoEntidad(lector, "Estado");
                                estaDadoDeBaja = estado == EstadoEntidad.Inactivo;
                            }
                        }
                    }
                    return estaDadoDeBaja;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


    }
}
