using SGTO.Dominio.Entidades;
using SGTO.Dominio.ObjetosValor;
using System;
using System.Data.SqlClient;


namespace SGTO.Datos.Mappers
{
    public static class UsuarioMapper
    {

        public static Usuario MapearAEntidad(SqlDataReader lector)
        {
            var usuario = new Usuario();

            if (TieneColumna(lector, "IdUsuario"))
                usuario.IdUsuario = lector.GetInt32(lector.GetOrdinal("IdUsuario"));

            if (TieneColumna(lector, "Apellido"))
                usuario.Apellido = lector["Apellido"]?.ToString();

            if (TieneColumna(lector, "Nombre"))
                usuario.Nombre = lector["Nombre"]?.ToString();

            if (TieneColumna(lector, "Email"))
                usuario.Email = new Email(lector["Email"].ToString());

            if (TieneColumna(lector, "NombreUsuario"))
                usuario.NombreUsuario = lector["NombreUsuario"]?.ToString();

            if (TieneColumna(lector, "IdRol"))
            {
                var rol = new Rol
                {
                    IdRol = Convert.ToInt32(lector["IdRol"]),
                    Nombre = TieneColumna(lector, "NombreRol") ? lector["NombreRol"].ToString() : null
                };
                usuario.Rol = rol;
            }

            if (TieneColumna(lector, "Estado"))
                usuario.Estado = EnumeracionMapperDatos.MapearEstadoEntidad(lector, "Estado");

            if (TieneColumna(lector, "PasswordHash") && !lector.IsDBNull(lector.GetOrdinal("PasswordHash")))
                usuario.PasswordHash = lector["PasswordHash"].ToString();

            if (TieneColumna(lector, "FechaAlta") && !lector.IsDBNull(lector.GetOrdinal("FechaAlta")))
                usuario.FechaAlta = lector.GetDateTime(lector.GetOrdinal("FechaAlta"));

            if (TieneColumna(lector, "FechaModificacion") && !lector.IsDBNull(lector.GetOrdinal("FechaModificacion")))
                usuario.FechaModificacion = lector.GetDateTime(lector.GetOrdinal("FechaModificacion"));

            return usuario;
        }


        private static bool TieneColumna(SqlDataReader lector, string nombreColumna)
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
