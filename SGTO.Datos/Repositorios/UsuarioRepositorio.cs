using SGTO.Datos.Infraestructura;
using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

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


        public bool ExisteNombreUsuario(string nombreUsuario)
        {
            string query = @"SELECT COUNT(*) FROM Usuario WHERE UPPER(NombreUsuario) = @NombreUsuario";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@NombreUsuario", nombreUsuario.ToUpper());

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                            return lector.GetInt32(0) > 0;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error en UsuarioRepositorio.ExisteNombreUsuario: " + ex.Message);
                    throw;
                }
            }
            return false;
        }

        public bool ExisteEmail(string email)
        {
            string query = @"SELECT COUNT(*) FROM Usuario WHERE UPPER(Email) = @Email";
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@Email", email.ToUpper());

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                            return lector.GetInt32(0) > 0;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error en UsuarioRepositorio.ExisteEmail: " + ex.Message);
                    throw;
                }
            }
            return false;
        }


        public int Crear(Usuario nuevoUsuario, ConexionDBFactory datos)
        {
            string query = @"
                INSERT INTO Usuario 
                    (Nombre, Apellido, Email, NombreUsuario, PasswordHash, IdRol, Estado, FechaAlta, FechaModificacion)
                OUTPUT INSERTED.IdUsuario
                VALUES
                    (@Nombre, @Apellido, @Email, @NombreUsuario, @PasswordHash, @IdRol, @Estado, @FechaAlta, @FechaModificacion)";
            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@Nombre", nuevoUsuario.Nombre);
            datos.EstablecerParametros("@Apellido", nuevoUsuario.Apellido);
            datos.EstablecerParametros("@Email", nuevoUsuario.Email.Valor);
            datos.EstablecerParametros("@NombreUsuario", nuevoUsuario.NombreUsuario);
            datos.EstablecerParametros("@PasswordHash", nuevoUsuario.PasswordHash);
            datos.EstablecerParametros("@IdRol", nuevoUsuario.Rol.IdRol);
            datos.EstablecerParametros("@Estado", nuevoUsuario.Estado.ToString()[0]);
            datos.EstablecerParametros("@FechaAlta", nuevoUsuario.FechaAlta);
            datos.EstablecerParametros("@FechaModificacion", nuevoUsuario.FechaModificacion);

            try
            {
                int id = datos.EjecutarAccionEscalar();
                return id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error en UsuarioRepositorio.Crear: " + ex.Message);
                throw;
            }
        }

        public Usuario ObtenerPorId(int idUsuario)
        {
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"
                    SELECT U.IdUsuario, U.Nombre, U.Apellido, U.Email, U.NombreUsuario,
                           U.PasswordHash, U.IdRol, R.Nombre AS NombreRol,
                           U.Estado, U.FechaAlta, U.FechaModificacion
                    FROM Usuario U
                    JOIN Rol R ON U.IdRol = R.IdRol
                    WHERE U.IdUsuario = @IdUsuario";

                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@IdUsuario", idUsuario);

                try
                {
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                            return UsuarioMapper.MapearAEntidad(lector);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error en UsuarioRepositorio.ObtenerPorId: " + ex.Message);
                    throw;
                }
            }
            return null;
        }

        public bool ExisteEmailEnOtroUsuario(string email, int idUsuario)
        {
            string query = @"SELECT COUNT(*) FROM Usuario WHERE UPPER(Email) = @Email AND IdUsuario <> @IdUsuario";
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@Email", email.ToUpper());
                    datos.EstablecerParametros("@IdUsuario", idUsuario);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                            return lector.GetInt32(0) > 0;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error en UsuarioRepositorio.ExisteEmailEnOtroUsuario: " + ex.Message);
                    throw;
                }
            }
            return false;
        }

        public bool ExisteNombreUsuarioEnOtroUsuario(string nombreUsuario, int idUsuario)
        {
            string query = @"SELECT COUNT(*) FROM Usuario WHERE UPPER(NombreUsuario) = @NombreUsuario AND IdUsuario <> @IdUsuario";
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@NombreUsuario", nombreUsuario.ToUpper());
                    datos.EstablecerParametros("@IdUsuario", idUsuario);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                            return lector.GetInt32(0) > 0;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error en UsuarioRepositorio.ExisteNombreUsuarioEnOtroUsuario: " + ex.Message);
                    throw;
                }
            }
            return false;
        }


        public void Editar(Usuario usuario, ConexionDBFactory datos)
        {
            string query = @"
                UPDATE Usuario
                SET Nombre = @Nombre,
                    Apellido = @Apellido,
                    Email = @Email,
                    NombreUsuario = @NombreUsuario,
                    IdRol = @IdRol,
                    Estado = @Estado,
                    FechaModificacion = @FechaModificacion
                    {{PASSWORD}}
                WHERE IdUsuario = @IdUsuario";

            if (!string.IsNullOrEmpty(usuario.PasswordHash))
                query = query.Replace("{{PASSWORD}}", ", PasswordHash = @PasswordHash");
            else
                query = query.Replace("{{PASSWORD}}", "");

            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@IdUsuario", usuario.IdUsuario);
            datos.EstablecerParametros("@Nombre", usuario.Nombre);
            datos.EstablecerParametros("@Apellido", usuario.Apellido);
            datos.EstablecerParametros("@Email", usuario.Email.Valor);
            datos.EstablecerParametros("@NombreUsuario", usuario.NombreUsuario);
            datos.EstablecerParametros("@IdRol", usuario.Rol.IdRol);
            datos.EstablecerParametros("@Estado", usuario.Estado.ToString()[0]);
            datos.EstablecerParametros("@FechaModificacion", usuario.FechaModificacion);

            if (!string.IsNullOrEmpty(usuario.PasswordHash))
                datos.EstablecerParametros("@PasswordHash", usuario.PasswordHash);

            try
            {
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error en UsuarioRepositorio.Editar: " + ex.Message);
                throw;
            }
        }


    }
}
