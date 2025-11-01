using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SGTO.Datos.Mappers
{
    public static class CoberturaMapper
    {

        public static Cobertura MapearAEntidad(SqlDataReader lector)
        {
            // usamos getordinal porque permite pedirle al lector el numero de la columna,
            // de esa forma solo se busca la primera vez (a través del nombre de la columna) y luego
            // solo usa el índice que hace que sea más rápido.
            int idCobertura = lector.GetInt32(lector.GetOrdinal("IdCobertura"));
            string nombre = lector.GetString(lector.GetOrdinal("Nombre"));
            string descripcion = lector.IsDBNull(lector.GetOrdinal("Descripcion"))
                ? string.Empty
                : lector.GetString(lector.GetOrdinal("Descripcion"));
            EstadoEntidad estado = EnumeracionMapper.MapearEstadoEntidad(lector, "Estado");

            Cobertura cobertura = new Cobertura(
                idCobertura,
                nombre,
                descripcion,
                estado
                );

            return cobertura;
        }
    }
}
