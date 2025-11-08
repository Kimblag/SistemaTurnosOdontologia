using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SGTO.Datos.Mappers
{
    public static class PlanMapper
    {
        public static Plan MapearAEntidad(SqlDataReader lector, Cobertura cobertura = null)
        {

            int idPlan = lector.GetInt32(lector.GetOrdinal("IdPlan"));
            string nombre = lector.GetString(lector.GetOrdinal("NombrePlan"));
            string descripcion = lector.IsDBNull(lector.GetOrdinal("DescripcionPlan"))
                ? string.Empty
                : lector.GetString(lector.GetOrdinal("DescripcionPlan"));
            decimal porcentajeCobertura = lector.GetDecimal(lector.GetOrdinal("PorcentajeCobertura"));
            EstadoEntidad estado = EnumeracionMapperDatos.MapearEstadoEntidad(lector, "EstadoPlan");

            Plan plan = new Plan(
                idPlan,
                nombre,
                descripcion,
                porcentajeCobertura,
                cobertura,
                estado
                );

            return plan;
        }

    }
}
