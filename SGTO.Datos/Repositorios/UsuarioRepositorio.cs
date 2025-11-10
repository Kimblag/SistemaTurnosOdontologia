using SGTO.Datos.Infraestructura;
using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SGTO.Datos.Repositorios
{
    public class UsuarioRepositorio
    {

        public List<Usuario> Listar(string estado = null)
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"
                                SELECT U.IdUsuario,
                                       U.Apellido,
                                       U.Nombre,
                                       U.Email,
                                       U.NombreUsuario,
                                       U.IdRol,
                                       R.Nombre AS NombreRol,
                                       U.Estado
                                FROM Usuario U
                                JOIN Rol R ON U.IdRol = R.IdRol
                                {{WHERE}}
                                ORDER BY U.Apellido ASC, U.Nombre ASC
                                ";
                query = estado != null
                    ? query.Replace("{{WHERE}}", $" WHERE U.Estado = '{estado[0]}'")
                    : query.Replace("{{WHERE}}", "");

                datos.DefinirConsulta(query);

                try
                {
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        while (lector.Read())
                        {
                            Usuario usuario = UsuarioMapper.MapearAEntidad(lector);
                            usuarios.Add(usuario);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return usuarios;
        }

    }
}
