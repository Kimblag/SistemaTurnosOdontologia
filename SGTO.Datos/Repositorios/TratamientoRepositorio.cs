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

    }
}