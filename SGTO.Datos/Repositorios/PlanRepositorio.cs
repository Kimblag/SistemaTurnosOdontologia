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

        public void DarDeBaja(int idCobertura, char estado, ConexionDBFactory datos)
        {
            string query = @"UPDATE [Plan] 
                                    SET Estado = @Estado
                                WHERE IdCobertura = @IdCobertura";

            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@Estado", estado);
            datos.EstablecerParametros("@IdCobertura", idCobertura);
            datos.EjecutarAccion();
        }

        public void ActualizarEstadoPorCobertura(int idCobertura, EstadoEntidad nuevoEstado, ConexionDBFactory datos)
        {
            string query = @"UPDATE [Plan]
                     SET Estado = @Estado
                     WHERE IdCobertura = @IdCobertura";

            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@Estado", EnumeracionMapper.ObtenerChar(nuevoEstado));
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
            datos.EstablecerParametros("@Estado", EnumeracionMapper.ObtenerChar(nuevoPlan.Estado));
            datos.EjecutarAccion();
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
                datos.EstablecerParametros("@Estado", EnumeracionMapper.ObtenerChar(nuevoPlan.Estado));
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
	                                C.Estado AS EstadoCobertura
                                FROM [Plan] P
                                LEFT JOIN Cobertura C ON P.IdCobertura = C.IdCobertura
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

        public bool EstadoDadoDeBaja(int idPlan)
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

        public void DarDeBaja(int idPlan, char estado)
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


    }
}
