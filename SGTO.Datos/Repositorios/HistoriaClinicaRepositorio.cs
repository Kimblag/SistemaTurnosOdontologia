using SGTO.Datos.Infraestructura;
using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SGTO.Datos.Repositorios
{
    public class HistoriaClinicaRepositorio
    {
        public HistoriaClinicaRegistro ObtenerPorIdTurno(int idTurno)
        {
            HistoriaClinicaRegistro historia = null;

            string query = @"
                SELECT 
                    H.IdHistoriaClinicaRegistro,
                    H.IdTurno,
                    H.FechaAtencion,
                    H.Diagnostico,
                    H.Observaciones,
                    H.IdTratamiento,
                    T.Nombre AS NombreTratamiento,
                    T.CostoBase
                FROM HistoriaClinicaRegistro H
                INNER JOIN Tratamiento T ON H.IdTratamiento = T.IdTratamiento
                WHERE H.IdTurno = @IdTurno";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                datos.LimpiarParametros();
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@IdTurno", idTurno);

                try
                {
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        // Solo debería haber 1 registro de historia por turno
                        if (lector.Read())
                        {
                            historia = HistoriaClinicaMapper.Mapear(lector);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return historia;
        }


        public List<HistoriaClinicaRegistro> ObtenerHistorialPorPaciente(int idPaciente)
        {
            var lista = new List<HistoriaClinicaRegistro>();

            string query = @"
                SELECT 
                    H.IdHistoriaClinicaRegistro,
                    H.IdTurno,
                    H.FechaAtencion,
                    H.Diagnostico,

                    T.IdTratamiento,
                    T.Nombre AS NombreTratamiento,

                    M.IdMedico,
                    M.Apellido + ', ' + M.Nombre AS NombreProfesional,

                    E.IdEspecialidad,
                    E.Nombre AS NombreEspecialidad
                FROM HistoriaClinicaRegistro H
                INNER JOIN Tratamiento T ON H.IdTratamiento = T.IdTratamiento
                INNER JOIN Medico M ON H.IdMedico = M.IdMedico
                INNER JOIN Especialidad E ON H.IdEspecialidad = E.IdEspecialidad
                WHERE H.IdPaciente = @IdPaciente
                ORDER BY H.FechaAtencion DESC";

            using (var datos = new ConexionDBFactory())
            {
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@IdPaciente", idPaciente);

                try
                {
                    using (var lector = datos.EjecutarConsulta())
                    {
                        while (lector.Read())
                        {
                            lista.Add(HistoriaClinicaMapper.Mapear(lector));
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return lista;
        }


    }
}
