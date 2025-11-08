using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using System;
using System.Data.SqlClient;

namespace SGTO.Datos.Mappers
{
    public static class TratamientoMapper
    {
        
        public static Tratamiento MapearAEntidad(SqlDataReader lector, Especialidad especialidad)
        {
            
            int idTratamiento = lector.GetInt32(lector.GetOrdinal("IdTratamiento"));
            string nombre = lector.GetString(lector.GetOrdinal("NombreTratamiento"));

            string descripcion = lector.IsDBNull(lector.GetOrdinal("DescripcionTratamiento"))
                ? string.Empty
                : lector.GetString(lector.GetOrdinal("DescripcionTratamiento"));

            decimal costoBase = lector.GetDecimal(lector.GetOrdinal("CostoBase"));

            EstadoEntidad estado = EnumeracionMapperDatos.MapearEstadoEntidad(lector, "EstadoTratamiento");

            
            Tratamiento tratamiento = new Tratamiento(
                idTratamiento,
                nombre,
                descripcion,
                costoBase,
                especialidad,
                estado
            );

            return tratamiento;
        }
    }
}