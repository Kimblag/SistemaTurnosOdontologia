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
    public class PermisoRepositorio
    {

        public List<Permiso> Listar()
        {
            List<Permiso> permisos = new List<Permiso>();

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"SELECT IdPermiso, Modulo, Accion, Descripcion
                                    FROM Permiso
                                 ORDER BY Modulo ASC, Accion ASC";

                try
                {
                    datos.DefinirConsulta(query);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        while (lector.Read())
                        {
                            permisos.Add(PermisoMapper.MapearAEntidad(lector));
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return permisos;
        }


        public Permiso ObtenerPorId(int idPermiso)
        {
            Permiso permiso = null;

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"SELECT IdPermiso, Modulo, Accion, Descripcion
                                    FROM Permiso
                                 WHERE IdPermiso = @IdPermiso";
                try
                {
                    datos.DefinirConsulta(query);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            permiso = PermisoMapper.MapearAEntidad(lector);
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return permiso;
        }

    }
}
