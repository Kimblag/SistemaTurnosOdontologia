using SGTO.Dominio.Entidades;
using System.Data.SqlClient;

namespace SGTO.Datos.Mappers
{
    public static class ParametroSistemaMapper
    {

        public static ParametroSistema MapearAEntidad(SqlDataReader lector)
        {
            int id = lector.GetInt32(lector.GetOrdinal("IdParametroSistema"));
            string nombre = lector.GetString(lector.GetOrdinal("Nombre"));
            string valor = lector.GetString(lector.GetOrdinal("Valor"));
            string descripcion = lector.GetString(lector.GetOrdinal("Descripcion"));

            return new ParametroSistema(id, nombre, valor, descripcion);
        }

    }
}
