using SGTO.Datos.Infraestructura;
using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SGTO.Datos.Repositorios
{
    public class EspecialidadRepositorio
    {
        public List<Especialidad> Listar(string estado = null)
        {
            List<Especialidad> especialidades = new List<Especialidad>();

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"SELECT E.IdEspecialidad, 
                                        E.Nombre AS NombreEspecialidad, 
                                        E.Descripcion AS DescripcionEspecialidad,
                                        E.Estado AS EstadoEspecialidad,
                                        T.IdTratamiento,
                                        T.Nombre AS NombreTratamiento,
                                        T.Descripcion AS DescripcionTratamiento,
                                        T.CostoBase,
                                        T.Estado AS EstadoTratamiento,
                                        T.IdEspecialidad AS IdEspecialidadTratamiento
                                    FROM Especialidad E
                                    LEFT JOIN Tratamiento T ON E.IdEspecialidad = T.IdEspecialidad";

                if (estado != null)
                {
                    query += $" WHERE LOWER(E.Estado) = LOWER('{estado.Substring(0, 1)}')";
                }

                try
                {
                    datos.DefinirConsulta(query);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        int idEspecialidadActual = -1;
                        Especialidad especialidadActual = null;

                        while (lector.Read())
                        {
                            int IdEspecialidad = lector.GetInt32(lector.GetOrdinal("IdEspecialidad"));

                            if (IdEspecialidad != idEspecialidadActual)
                            {
                                especialidadActual = EspecialidadMapper.MapearAEntidad(lector);
                                especialidades.Add(especialidadActual);

                                idEspecialidadActual = IdEspecialidad;
                            }

                            if (!lector.IsDBNull(lector.GetOrdinal("IdTratamiento")))
                            {
                                Tratamiento tratamiento = TratamientoMapper.MapearAEntidad(lector, especialidadActual);

                                especialidadActual.TratamientosAsociados.Add(tratamiento);
                            }
                        }
                    }

                    return especialidades;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void Crear(Especialidad especialidad)
        {
           string query = @"INSERT INTO Especialidad (Nombre, Descripcion, Estado)
                             VALUES (@Nombre, @Descripcion, @Estado)";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@Nombre", especialidad.Nombre);
                    datos.EstablecerParametros("@Descripcion", especialidad.Descripcion);
                    datos.EstablecerParametros("@Estado", (char)especialidad.Estado);
                    datos.EjecutarAccion();
                }
                catch (Exception)
                {

                    throw;
                }
                            
            }

        }


        public Especialidad ObtenerPorId(int id)
        {
            Especialidad especialidad = null; 

            string query = @"SELECT IdEspecialidad, 
                            Nombre AS NombreEspecialidad, 
                            Descripcion AS DescripcionEspecialidad,
                            Estado AS EstadoEspecialidad
                        FROM Especialidad
                        WHERE IdEspecialidad = @IdEspecialidad";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@IdEspecialidad", id);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            especialidad = EspecialidadMapper.MapearAEntidad(lector);
                        }
                    }

                    return especialidad;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public void Modificar(Especialidad especialidad)
        {
            string query = @"UPDATE Especialidad 
                     SET Nombre = @Nombre, 
                         Descripcion = @Descripcion, 
                         Estado = @Estado
                     WHERE IdEspecialidad = @IdEspecialidad";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@Nombre", especialidad.Nombre);
                    datos.EstablecerParametros("@Descripcion", especialidad.Descripcion);
                    datos.EstablecerParametros("@Estado", (char)especialidad.Estado);
                    datos.EstablecerParametros("@IdEspecialidad", especialidad.IdEspecialidad);
                    datos.EjecutarAccion();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

    }
}