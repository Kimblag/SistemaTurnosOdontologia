using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using System;
using System.Data.SqlClient;


namespace SGTO.Datos.Mappers
{
    public static class UsuarioMapper
    {

        public static Usuario MapearAEntidad(SqlDataReader lector)
        {
            int idUsuario = lector.GetInt32(lector.GetOrdinal("IdUsuario"));
            string apellido = lector.GetString(lector.GetOrdinal("Apellido"));
            string nombre = lector.GetString(lector.GetOrdinal("Nombre"));
            string email = lector.GetString(lector.GetOrdinal("Email"));
            string nombreUsuario = lector.GetString(lector.GetOrdinal("NombreUsuario"));
            int idRol = lector.GetInt32(lector.GetOrdinal("IdRol"));
            string nombreRol = lector.GetString(lector.GetOrdinal("NombreRol"));
            EstadoEntidad estado = EnumeracionMapperDatos.MapearEstadoEntidad(lector, "Estado");


            Usuario usuario = new Usuario()
            {
                IdUsuario = idUsuario,
                Apellido = apellido,
                Nombre = nombre,
                Email = new Email(email),
                NombreUsuario = nombreUsuario,
                Rol = new Rol() { IdRol = idRol, Nombre = nombreRol },
                Estado = estado,
            };
            return usuario;
        }

    }
}
