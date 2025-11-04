using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SGTO.Datos.Mappers
{
    public static class EspecialidadMapper
    {
        public static Especialidad MapearAEntidad(SqlDataReader lector)
        {
            int idEspecialidad = lector.GetInt32(lector.GetOrdinal("IdEspecialidad"));
            string nombre = lector.GetString(lector.GetOrdinal("NombreEspecialidad"));
            string descripcion = lector.IsDBNull(lector.GetOrdinal("DescripcionEspecialidad"))
                ? string.Empty
                : lector.GetString(lector.GetOrdinal("DescripcionEspecialidad"));
            EstadoEntidad estado = EnumeracionMapper.MapearEstadoEntidad(lector, "EstadoEspecialidad");

          
            Especialidad especialidad = new Especialidad(
                idEspecialidad,
                nombre,
                descripcion,
                new List<Tratamiento>(),
                estado
            );

            return especialidad;
        }
    }
}