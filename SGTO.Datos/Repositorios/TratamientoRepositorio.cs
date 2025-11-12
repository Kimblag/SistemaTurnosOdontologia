using SGTO.Datos.Infraestructura;
using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SGTO.Datos.Repositorios
{
    public class TratamientoRepositorio
    {
        public List<Tratamiento> Listar(string estado = null)
        {
            List<Tratamiento> tratamientos = new List<Tratamiento>();
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"SELECT T.IdTratamiento, 
                                        T.Nombre AS NombreTratamiento, 
                                        T.Descripcion AS DescripcionTratamiento,
                                        T.CostoBase,
                                        T.Estado AS EstadoTratamiento,
                                        E.IdEspecialidad,
                                        E.Nombre AS NombreEspecialidad,
                                        E.Descripcion AS DescripcionEspecialidad,
                                        E.Estado AS EstadoEspecialidad
                                 FROM Tratamiento T
                                 INNER JOIN Especialidad E ON T.IdEspecialidad = E.IdEspecialidad";

                if (estado != null)
                {
                    query += $" WHERE LOWER(T.Estado) = LOWER('{estado.Substring(0, 1)}')";
                }
                try
                {
                    datos.DefinirConsulta(query);
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        while (lector.Read())
                        {
                            Especialidad especialidad = EspecialidadMapper.MapearAEntidad(lector);
                            Tratamiento tratamiento = TratamientoMapper.MapearAEntidad(lector, especialidad);
                            tratamientos.Add(tratamiento);
                        }
                    }
                    return tratamientos;
                }
                catch (Exception) { throw; }
            }
        }

        public Tratamiento ObtenerPorId(int id)
        {
            Tratamiento tratamiento = null;

            string query = @"SELECT T.IdTratamiento, 
                                    T.Nombre AS NombreTratamiento, 
                                    T.Descripcion AS DescripcionTratamiento,
                                    T.CostoBase,
                                    T.Estado AS EstadoTratamiento,
                                    E.IdEspecialidad,
                                    E.Nombre AS NombreEspecialidad,
                                    E.Descripcion AS DescripcionEspecialidad,
                                    E.Estado AS EstadoEspecialidad
                             FROM Tratamiento T
                             INNER JOIN Especialidad E ON T.IdEspecialidad = E.IdEspecialidad
                             WHERE T.IdTratamiento = @IdTratamiento";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@IdTratamiento", id);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            tratamiento = TratamientoMapper.MapearAEntidad(lector,
                                EspecialidadMapper.MapearAEntidad(lector));

                        }
                    }
                    return tratamiento;
                }
                catch (Exception) { throw; }
            }
        }

        public void Crear(Tratamiento tratamiento)
        {
            string query = @"INSERT INTO Tratamiento (Nombre, Descripcion, CostoBase, Estado, IdEspecialidad)
                             VALUES (@Nombre, @Descripcion, @CostoBase, @Estado, @IdEspecialidad)";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@Nombre", tratamiento.Nombre);
                    datos.EstablecerParametros("@Descripcion", tratamiento.Descripcion);
                    datos.EstablecerParametros("@CostoBase", tratamiento.CostoBase);
                    datos.EstablecerParametros("@Estado", tratamiento.Estado.ToString().Substring(0, 1));
                    datos.EstablecerParametros("@IdEspecialidad", tratamiento.Especialidad.IdEspecialidad);
                    datos.EjecutarAccion();
                }
                catch (Exception) { throw; }
            }

        }

    }
}