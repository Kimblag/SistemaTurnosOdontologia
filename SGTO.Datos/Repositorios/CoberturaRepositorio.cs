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
                try
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
    }
}
