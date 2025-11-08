using SGTO.Datos.Infraestructura;
using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SGTO.Datos.Repositorios
{
    public class PlanRepositorio
    {

        public void ActualizarEstadoPorCobertura(int idCobertura, EstadoEntidad nuevoEstado, ConexionDBFactory datos)
        {
            string query = @"UPDATE [Plan]
                     SET Estado = @Estado
                     WHERE IdCobertura = @IdCobertura";

            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@Estado", nuevoEstado.ToString()[0]);
            datos.EstablecerParametros("@IdCobertura", idCobertura);
            datos.EjecutarAccion();
        }

        public void Crear(Plan nuevoPlan, ConexionDBFactory datos)
        {
            string query = @"INSERT INTO [Plan] (Nombre, Descripcion, PorcentajeCobertura, IdCobertura, Estado)
                                VALUES (@Nombre, @Descripcion, @PorcentajeCobertura, @IdCobertura, @Estado)";
            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@Nombre", nuevoPlan.Nombre);
            datos.EstablecerParametros("@Descripcion", nuevoPlan.Descripcion);
            datos.EstablecerParametros("@PorcentajeCobertura", nuevoPlan.PorcentajeCobertura);
            datos.EstablecerParametros("@IdCobertura", nuevoPlan.Cobertura.IdCobertura);
            datos.EstablecerParametros("@Estado", nuevoPlan.Estado.ToString()[0]);
            datos.EjecutarAccion();
        }

        public void Crear(Plan nuevoPlan)
        {
            string query = @"INSERT INTO [Plan] (Nombre, Descripcion, PorcentajeCobertura, IdCobertura, Estado)
                                VALUES (@Nombre, @Descripcion, @PorcentajeCobertura, @IdCobertura, @Estado)";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@Nombre", nuevoPlan.Nombre);
                    datos.EstablecerParametros("@Descripcion", nuevoPlan.Descripcion);
                    datos.EstablecerParametros("@PorcentajeCobertura", nuevoPlan.PorcentajeCobertura);
                    datos.EstablecerParametros("@IdCobertura", nuevoPlan.Cobertura.IdCobertura);
                    datos.EstablecerParametros("@Estado", nuevoPlan.Estado.ToString()[0]);
                    datos.EjecutarAccion();
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }


        public void Crear(List<Plan> planes, ConexionDBFactory datos)
        {
            // método para crear planes a partir de una lista.

            string query = @"INSERT INTO [Plan] (Nombre, Descripcion, PorcentajeCobertura, IdCobertura, Estado)
                     VALUES (@Nombre, @Descripcion, @PorcentajeCobertura, @IdCobertura, @Estado)";

            foreach (Plan nuevoPlan in planes)
            {
                datos.LimpiarParametros();
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@Nombre", nuevoPlan.Nombre);
                datos.EstablecerParametros("@Descripcion", nuevoPlan.Descripcion);
                datos.EstablecerParametros("@PorcentajeCobertura", nuevoPlan.PorcentajeCobertura);
                datos.EstablecerParametros("@IdCobertura", nuevoPlan.Cobertura.IdCobertura);
                datos.EstablecerParametros("@Estado", nuevoPlan.Estado.ToString()[0]);
                datos.EjecutarAccion();
            }
        }

        public List<Plan> Listar(string estado = null)
        {
            List<Plan> planes = new List<Plan>();

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {

                string query = @"SELECT P.IdPlan,
	                                P.Nombre AS NombrePlan,
	                                P.Descripcion AS DescripcionPlan,
	                                P.PorcentajeCobertura,
	                                P.Estado AS EstadoPlan,
                                    C.IdCobertura, 
	                                C.Nombre AS NombreCobertura, 
	                                C.Descripcion AS DescripcionCobertura,
                                    CPH.PorcentajeCobertura AS PorcentajeCoberturaVigente,
	                                C.Estado AS EstadoCobertura
                                FROM [Plan] P
                                LEFT JOIN Cobertura C ON P.IdCobertura = C.IdCobertura
                                LEFT JOIN CoberturaPorcentajeHistorial CPH 
                                     ON CPH.IdCobertura = C.IdCobertura AND CPH.Estado = 'A'
                                {{WHERE}}                                
                                ORDER BY P.Nombre ASC";

                query = estado != null
                    ? query.Replace("{{WHERE}}", $" WHERE UPPER(P.Estado) = UPPER('{estado[0]}')")
                    : query.Replace("{{WHERE}}", " ");

                try
                {
                    datos.DefinirConsulta(query);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        while (lector.Read())
                        {
                            Cobertura cobertura = CoberturaMapper.MapearAEntidad(lector);
                            Plan plan = PlanMapper.MapearAEntidad(lector, cobertura);

                            planes.Add(plan);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return planes;
        }
        public List<Plan> ListarPorCobertura(int idCobertura, string estado = null)
        {
            List<Plan> planes = new List<Plan>();

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {

                string query = @"SELECT P.IdPlan,
	                                P.Nombre AS NombrePlan,
	                                P.Descripcion AS DescripcionPlan,
	                                P.PorcentajeCobertura,
	                                P.Estado AS EstadoPlan,
                                    C.IdCobertura, 
	                                C.Nombre AS NombreCobertura, 
	                                C.Descripcion AS DescripcionCobertura,
                                    CPH.PorcentajeCobertura AS PorcentajeCoberturaVigente,
	                                C.Estado AS EstadoCobertura
                                FROM [Plan] P
                                LEFT JOIN Cobertura C ON P.IdCobertura = C.IdCobertura
                                LEFT JOIN CoberturaPorcentajeHistorial CPH 
                                     ON CPH.IdCobertura = C.IdCobertura AND CPH.Estado = 'A'
                                WHERE C.IdCobertura = @IdCobertura
                                {{FILTROS}}
                                ORDER BY P.Nombre ASC";

                query = estado != null
                    ? query.Replace("{{FILTROS}}", $" AND UPPER(P.Estado) = UPPER('{estado[0]}')")
                    : query.Replace("{{FILTROS}}", " ");

                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@IdCobertura", idCobertura);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        while (lector.Read())
                        {
                            Cobertura cobertura = CoberturaMapper.MapearAEntidad(lector);
                            Plan plan = PlanMapper.MapearAEntidad(lector, cobertura);

                            planes.Add(plan);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return planes;
        }

        public bool EstaDadoDeBaja(int idPlan)
        {
            bool estaDadoDeBaja = false;
            string query = @"SELECT Estado
                                FROM [Plan]
                            WHERE IdPlan = @IdPlan";
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@IdPlan", idPlan);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            if (!lector.IsDBNull(lector.GetOrdinal("Estado")))
                            {
                                EstadoEntidad estado = EnumeracionMapperDatos.MapearEstadoEntidad(lector, "Estado");
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

        public void ActualizarEstado(int idPlan, char estado)
        {
            string query = @"UPDATE [Plan] SET Estado = @Estado WHERE IdPlan = @IdPlan";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@Estado", estado);
                    datos.EstablecerParametros("@IdPlan", idPlan);
                    datos.EjecutarAccion();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        public Plan ObtenerPorId(int idPlan)
        {
            Plan plan = new Plan();
            string query = @"SELECT P.IdPlan,
	                                P.Nombre AS NombrePlan,
	                                P.Descripcion AS DescripcionPlan,
	                                P.PorcentajeCobertura,
	                                P.Estado AS EstadoPlan,
                                    C.IdCobertura, 
	                                C.Nombre AS NombreCobertura, 
	                                C.Descripcion AS DescripcionCobertura,
                                    CPH.PorcentajeCobertura AS PorcentajeCoberturaVigente,
	                                C.Estado AS EstadoCobertura
                                FROM [Plan] P
                                LEFT JOIN Cobertura C ON P.IdCobertura = C.IdCobertura
                                LEFT JOIN CoberturaPorcentajeHistorial CPH 
                                    ON CPH.IdCobertura = C.IdCobertura 
                                    AND CPH.Estado = 'A'
                                WHERE IdPlan = @IdPlan";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@IdPlan", idPlan);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            Cobertura cobertura = CoberturaMapper.MapearAEntidad(lector);
                            plan = PlanMapper.MapearAEntidad(lector, cobertura);

                        }
                    }
                    return plan;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        public bool ExistePorNombre(string nombrePlan, int idCobertura)
        {
            bool resultado = false;
            string query = @"SELECT COUNT(Nombre)
                                FROM [Plan]
                            WHERE UPPER(Nombre) = @Nombre AND IdCobertura = @IdCobertura";
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@Nombre", nombrePlan.ToUpper());
                    datos.EstablecerParametros("@IdCobertura", idCobertura);

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


        public void Modificar(Plan plan)
        {
            string query = @"UPDATE [Plan] 
                                SET Nombre = @Nombre, 
                                    Descripcion = @Descripcion,
                                    PorcentajeCobertura = @PorcentajeCobertura,
                                    Estado = @Estado
                            WHERE IdPlan = @IdPlan";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@IdPlan", plan.IdPlan);
                    datos.EstablecerParametros("@Nombre", plan.Nombre);
                    datos.EstablecerParametros("@Descripcion", plan.Descripcion);
                    datos.EstablecerParametros("@PorcentajeCobertura", plan.PorcentajeCobertura);
                    datos.EstablecerParametros("@Estado", plan.Estado.ToString()[0]);

                    datos.EjecutarAccion();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }




    }
}
