using SGTO.Dominio.Entidades;
using System.Data.SqlClient;

namespace SGTO.Datos.Mappers
{
    public static class PermisoMapper
    {
        public static Permiso MapearAEntidad(SqlDataReader lector)
        {
            Permiso permiso = new Permiso();
            permiso.IdPermiso = lector.GetInt32(lector.GetOrdinal("IdPermiso"));
            permiso.Modulo = EnumeracionMapperDatos.MapearModulo(lector, "Modulo");
            permiso.Accion = EnumeracionMapperDatos.MapearTipoAccion(lector, "Accion");
            permiso.Descripcion = lector.IsDBNull(lector.GetOrdinal("Descripcion"))
                ? string.Empty
                : lector.GetString(lector.GetOrdinal("Descripcion"));
            return permiso;
        }
    }
}
