using SGTO.Datos.Infraestructura;
using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Datos.Repositorios
{
    public class RolRepositorio
    {

        public List<Rol> Listar(string estado = null, string nombre = null)
        {
            List<Rol> roles = new List<Rol>();

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"SELECT R.IdRol,
                                R.Nombre,
                                R.Descripcion,
                                R.Estado,
                                RP.IdPermiso,
                                P.Modulo,
                                P.Accion,
                                P.Descripcion AS DescripcionPermiso
                         FROM Rol R
                         LEFT JOIN RolPermiso RP ON R.IdRol = RP.IdRol
                         LEFT JOIN Permiso P ON RP.IdPermiso = P.IdPermiso
                         {{WHERE}}
                         ORDER BY R.Nombre ASC";

                string where = " ";
                if (estado != null)
                {
                    where = " WHERE UPPER(R.Estado) = UPPER(@Estado)";
                }

                if (!string.IsNullOrEmpty(nombre))
                {
                    if (where.Trim().StartsWith("WHERE"))
                        where += " AND UPPER(R.Nombre) LIKE @Nombre";
                    else
                        where = " WHERE UPPER(R.Nombre) LIKE @Nombre";
                }


                query = query.Replace("{{WHERE}}", where);


                datos.DefinirConsulta(query);

                if (estado != null)
                    datos.EstablecerParametros("@Estado", estado[0]);
                if (!string.IsNullOrEmpty(nombre))
                    datos.EstablecerParametros("@Nombre", "%" + nombre.ToUpper() + "%");

                using (SqlDataReader lector = datos.EjecutarConsulta())
                {
                    int idRolActual = -1;
                    Rol rolActual = null;

                    while (lector.Read())
                    {
                        int idRol = lector.GetInt32(lector.GetOrdinal("IdRol"));

                        if (idRol != idRolActual)
                        {
                            rolActual = RolMapper.MapearAEntidad(lector);
                            rolActual.Permisos = new List<Permiso>();
                            roles.Add(rolActual);
                            idRolActual = idRol;
                        }

                        if (!lector.IsDBNull(lector.GetOrdinal("IdPermiso")))
                        {
                            Permiso permiso = new Permiso();
                            permiso.IdPermiso = lector.GetInt32(lector.GetOrdinal("IdPermiso"));
                            permiso.Modulo = EnumeracionMapperDatos.MapearModulo(lector, "Modulo");
                            permiso.Accion = EnumeracionMapperDatos.MapearTipoAccion(lector, "Accion");
                            permiso.Descripcion = lector.IsDBNull(lector.GetOrdinal("DescripcionPermiso"))
                                ? string.Empty
                                : lector.GetString(lector.GetOrdinal("DescripcionPermiso"));

                            rolActual.Permisos.Add(permiso);
                        }
                    }
                }
                return roles;
            }
        }


        public Rol ObtenerPorId(int idRol)
        {
            Rol rol = null;

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"SELECT R.IdRol,
                                R.Nombre,
                                R.Descripcion,
                                R.Estado,
                                RP.IdPermiso,
                                P.Modulo,
                                P.Accion,
                                P.Descripcion AS DescripcionPermiso
                         FROM Rol R
                         LEFT JOIN RolPermiso RP ON R.IdRol = RP.IdRol
                         LEFT JOIN Permiso P ON RP.IdPermiso = P.IdPermiso
                         WHERE R.IdRol = @IdRol";

                try
                {
                    datos.DefinirConsulta(query);
                    datos.LimpiarParametros();
                    datos.EstablecerParametros("@IdRol", idRol);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        while (lector.Read())
                        {
                            if (rol == null)
                            {
                                rol = RolMapper.MapearAEntidad(lector);
                                rol.Permisos = new List<Permiso>();
                            }

                            if (!lector.IsDBNull(lector.GetOrdinal("IdPermiso")))
                            {
                                Permiso permiso = new Permiso();
                                permiso.IdPermiso = lector.GetInt32(lector.GetOrdinal("IdPermiso"));
                                permiso.Modulo = EnumeracionMapperDatos.MapearModulo(lector, "Modulo");
                                permiso.Accion = EnumeracionMapperDatos.MapearTipoAccion(lector, "Accion");
                                permiso.Descripcion = lector.IsDBNull(lector.GetOrdinal("DescripcionPermiso"))
                                    ? string.Empty
                                    : lector.GetString(lector.GetOrdinal("DescripcionPermiso"));

                                rol.Permisos.Add(permiso);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return rol;
        }


        public bool ExistePorNombre(string nombreRol)
        {
            bool existe = false;

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"SELECT COUNT(*) FROM Rol WHERE UPPER(Nombre) = @Nombre";

                try
                {
                    datos.DefinirConsulta(query);
                    datos.LimpiarParametros();
                    datos.EstablecerParametros("@Nombre", nombreRol.ToUpper());

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            existe = lector.GetInt32(0) > 0;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }

            }
            return existe;
        }


        public int Crear(Rol rol, ConexionDBFactory datos)
        {
            string query = @"INSERT INTO Rol (Nombre, Descripcion, Estado)
                     OUTPUT INSERTED.IdRol
                     VALUES (@Nombre, @Descripcion, @Estado)";

            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@Nombre", rol.Nombre);
            datos.EstablecerParametros("@Descripcion", rol.Descripcion);
            datos.EstablecerParametros("@Estado", rol.Estado.ToString()[0]);

            int idNuevoRol = datos.EjecutarAccionEscalar();
            return idNuevoRol;
        }


        public void Modificar(Rol rol, ConexionDBFactory datos)
        {
            string query = @"UPDATE Rol
                     SET Nombre = @Nombre,
                         Descripcion = @Descripcion,
                         Estado = @Estado
                     WHERE IdRol = @IdRol";

            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@Nombre", rol.Nombre);
            datos.EstablecerParametros("@Descripcion", rol.Descripcion);
            datos.EstablecerParametros("@Estado", rol.Estado.ToString()[0]);
            datos.EstablecerParametros("@IdRol", rol.IdRol);
            datos.EjecutarAccion();
        }


        public void EliminarPermisosPorRol(int idRol, ConexionDBFactory datos)
        {
            string query = @"DELETE FROM RolPermiso WHERE IdRol = @IdRol";
            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@IdRol", idRol);
            datos.EjecutarAccion();
        }


        public void InsertarPermisoARol(int idRol, int idPermiso, ConexionDBFactory datos)
        {
            string query = @"INSERT INTO RolPermiso (IdRol, IdPermiso)
                     VALUES (@IdRol, @IdPermiso)";
            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@IdRol", idRol);
            datos.EstablecerParametros("@IdPermiso", idPermiso);
            datos.EjecutarAccion();
        }

        public void DarDeBaja(int idRol)
        {
            string query = @"UPDATE Rol SET Estado = 'I' WHERE IdRol = @IdRol";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@IdRol", idRol);
                    datos.EjecutarAccion();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        public bool TieneUsuariosAsociados(int idRol)
        {
            bool tiene = false;
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"SELECT COUNT(*) FROM Usuario WHERE IdRol = @IdRol AND Estado = 'A'";
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@IdRol", idRol);

                using (SqlDataReader lector = datos.EjecutarConsulta())
                {
                    if (lector.Read())
                    {
                        tiene = lector.GetInt32(0) > 0;
                    }
                }
            }
            return tiene;
        }

    }
}
