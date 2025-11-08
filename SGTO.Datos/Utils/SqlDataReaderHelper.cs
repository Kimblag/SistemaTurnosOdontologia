using System;
using System.Data.SqlClient;

namespace SGTO.Datos.Utils
{
    public static class SqlDataReaderHelper
    {
        public static bool TieneColumna(SqlDataReader lector, string nombreColumna)
        {
            for (int i = 0; i < lector.FieldCount; i++)
            {
                if (lector.GetName(i).Equals(nombreColumna, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
