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


        public void CrearTransaccional(HistoriaClinicaRegistro historia)
        {
            string query = @"
                BEGIN TRANSACTION;

                BEGIN TRY
                    INSERT INTO HistoriaClinicaRegistro 
                    (IdTurno, IdPaciente, IdMedico, IdEspecialidad, IdTratamiento, TratamientoManual, Diagnostico, Observaciones, FechaAtencion)
                    VALUES 
                    (@IdTurno, @IdPaciente, @IdMedico, @IdEspecialidad, @IdTratamiento, @TratamientoManual, @Diagnostico, @Observaciones, @FechaAtencion);

                    UPDATE Turno 
                    SET Estado = 'Z', 
                        FechaModificacion = GETDATE()
                    WHERE IdTurno = @IdTurno;

                    COMMIT TRANSACTION;
                END TRY
                BEGIN CATCH
                    IF @@TRANCOUNT > 0
                        ROLLBACK TRANSACTION;
                    THROW; 
                END CATCH
            ";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.DefinirConsulta(query);

                    datos.EstablecerParametros("@IdTurno", historia.TurnoOrigen.IdTurno);

                    datos.EstablecerParametros("@IdPaciente", historia.TurnoOrigen.Paciente.IdPaciente);
                    datos.EstablecerParametros("@IdMedico", historia.Medico.IdMedico);
                    datos.EstablecerParametros("@IdEspecialidad", historia.Especialidad.IdEspecialidad);
                    datos.EstablecerParametros("@FechaAtencion", historia.FechaAtencion);
                    datos.EstablecerParametros("@Diagnostico", historia.Diagnostico);
                    if (string.IsNullOrEmpty(historia.Observaciones))
                        datos.EstablecerParametros("@Observaciones", DBNull.Value);
                    else
                        datos.EstablecerParametros("@Observaciones", historia.Observaciones);

                    if (historia.TratamientoAplicado != null && historia.TratamientoAplicado.IdTratamiento > 0)
                    {
                        datos.EstablecerParametros("@IdTratamiento", historia.TratamientoAplicado.IdTratamiento);
                    }
                    else
                    {
                        datos.EstablecerParametros("@IdTratamiento", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(historia.TratamientoManual))
                    {
                        datos.EstablecerParametros("@TratamientoManual", historia.TratamientoManual);
                    }
                    else
                    {
                        datos.EstablecerParametros("@TratamientoManual", DBNull.Value);
                    }


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
