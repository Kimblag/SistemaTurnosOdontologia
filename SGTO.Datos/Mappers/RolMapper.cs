using SGTO.Dominio.Entidades;
using System.Data.SqlClient;

namespace SGTO.Datos.Mappers
{
    public static class RolMapper
    {
        public static Rol MapearAEntidad(SqlDataReader lector)
        {
            Rol rol = new Rol();
            rol.IdRol = lector.GetInt32(lector.GetOrdinal("IdRol"));
            rol.Nombre = lector.GetString(lector.GetOrdinal("Nombre"));
            rol.Descripcion = lector.IsDBNull(lector.GetOrdinal("Descripcion"))
                ? string.Empty
                : lector.GetString(lector.GetOrdinal("Descripcion"));
            rol.Estado = EnumeracionMapperDatos.MapearEstadoEntidad(lector, "Estado");

            return rol;
        }
    }
}
