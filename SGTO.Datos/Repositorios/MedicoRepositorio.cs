using SGTO.Datos.Infraestructura;
using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using System;
using System.Data.SqlClient;
using System.Diagnostics;


namespace SGTO.Datos.Repositorios
{
    public class MedicoRepositorio
    {

        public bool ExistePorMatricula(string matricula)
        {
            string query = @"SELECT COUNT(*) FROM Medico WHERE UPPER(Matricula) = @Matricula";
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@Matricula", matricula.ToUpper());

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                            return lector.GetInt32(0) > 0;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error en MedicoRepositorio existePorMatricula: " + ex.Message);
                    throw;
                }
            }
            return false;
        }


        public bool ExistePorDocumento(string dni)
        {
            string query = @"SELECT COUNT(*) FROM Medico WHERE NumeroDocumento = @Dni";
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@Dni", dni);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                            return lector.GetInt32(0) > 0;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error en MedicoRepositorio ExistePorDocumento: " + ex.Message);
                    throw;
                }
            }

            return false;
        }


        public void Crear(Medico nuevoMedico, ConexionDBFactory datos)
        {
            string query = @"
                INSERT INTO Medico 
                    (Nombre, Apellido, NumeroDocumento, Genero, FechaNacimiento, Telefono, Email, 
                     Matricula, IdUsuario, IdEspecialidad, Estado, FechaAlta, FechaModificacion)
                VALUES
                    (@Nombre, @Apellido, @NumeroDocumento, @Genero, @FechaNacimiento, @Telefono, @Email, 
                     @Matricula, @IdUsuario, @IdEspecialidad, @Estado, @FechaAlta, @FechaModificacion)";

            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@Nombre", nuevoMedico.Nombre);
            datos.EstablecerParametros("@Apellido", nuevoMedico.Apellido);
            datos.EstablecerParametros("@NumeroDocumento", nuevoMedico.Dni.Numero);
            datos.EstablecerParametros("@Genero", nuevoMedico.Genero.ToString()[0]);
            datos.EstablecerParametros("@FechaNacimiento", nuevoMedico.FechaNacimiento);
            datos.EstablecerParametros("@Telefono", nuevoMedico.Telefono.Numero);
            datos.EstablecerParametros("@Email", nuevoMedico.Email.Valor);
            datos.EstablecerParametros("@Matricula", nuevoMedico.Matricula);
            datos.EstablecerParametros("@IdUsuario", nuevoMedico.Usuario.IdUsuario);
            datos.EstablecerParametros("@IdEspecialidad",
                (nuevoMedico.Especialidades != null && nuevoMedico.Especialidades.Count > 0)
                ? (object)nuevoMedico.Especialidades[0].IdEspecialidad
                : DBNull.Value);
            datos.EstablecerParametros("@Estado", nuevoMedico.Estado.ToString()[0]);
            datos.EstablecerParametros("@FechaAlta", nuevoMedico.FechaAlta);
            datos.EstablecerParametros("@FechaModificacion", nuevoMedico.FechaModificacion);

            try
            {
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error en MedicoRepositorio.Crear: " + ex.Message);
                throw;
            }
        }

        public Medico ObtenerPorUsuarioId(int idUsuario)
        {
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"
                                SELECT M.IdMedico, M.Nombre, M.Apellido, M.NumeroDocumento, M.Genero, 
                                        M.FechaNacimiento, M.Telefono, M.Email, M.Matricula, 
                                        M.IdUsuario, M.IdEspecialidad, E.Nombre AS NombreEspecialidad, M.Estado, M.FechaAlta, M.FechaModificacion
                                FROM Medico M
                                JOIN Especialidad E ON M.IdEspecialidad = E.IdEspecialidad
                                WHERE M.IdUsuario = @IdUsuario";

                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@IdUsuario", idUsuario);

                try
                {
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            var medico = MedicoMapper.MapearAEntidad(lector, idUsuario);

                            if (!lector.IsDBNull(lector.GetOrdinal("IdEspecialidad")))
                            {
                                int idEsp = lector.GetInt32(lector.GetOrdinal("IdEspecialidad"));
                                string nombreEspecialidad = lector.GetString(lector.GetOrdinal("NombreEspecialidad"));
                                medico.Especialidades.Add(new Especialidad { IdEspecialidad = idEsp, Nombre = nombreEspecialidad });
                            }
                            return medico;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error en MedicoRepositorio.ObtenerPorUsuarioId: " + ex.Message);
                    throw;
                }
            }
            return null;
        }

        public void Editar(Medico medico, ConexionDBFactory datos)
        {
            string query = @"
                UPDATE Medico
                SET Nombre = @Nombre,
                    Apellido = @Apellido,
                    NumeroDocumento = @NumeroDocumento,
                    Genero = @Genero,
                    FechaNacimiento = @FechaNacimiento,
                    Telefono = @Telefono,
                    Email = @Email,
                    Matricula = @Matricula,
                    IdEspecialidad = @IdEspecialidad,
                    Estado = @Estado,
                    FechaModificacion = @FechaModificacion
                WHERE IdUsuario = @IdUsuario";

            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@Nombre", medico.Nombre);
            datos.EstablecerParametros("@Apellido", medico.Apellido);
            datos.EstablecerParametros("@NumeroDocumento", medico.Dni.Numero);
            datos.EstablecerParametros("@Genero", medico.Genero.ToString()[0]);
            datos.EstablecerParametros("@FechaNacimiento", medico.FechaNacimiento);
            datos.EstablecerParametros("@Telefono", medico.Telefono.Numero);
            datos.EstablecerParametros("@Email", medico.Email.Valor);
            datos.EstablecerParametros("@Matricula", medico.Matricula);
            datos.EstablecerParametros("@IdEspecialidad",
                (medico.Especialidades != null && medico.Especialidades.Count > 0)
                ? (object)medico.Especialidades[0].IdEspecialidad
                : DBNull.Value);
            datos.EstablecerParametros("@Estado", medico.Estado.ToString()[0]);
            datos.EstablecerParametros("@FechaModificacion", medico.FechaModificacion);
            datos.EstablecerParametros("@IdUsuario", medico.Usuario.IdUsuario);

            try
            {
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error en MedicoRepositorio.Editar: " + ex.Message);
                throw;
            }
        }

        public bool ExistePorMatriculaEnOtro(string matricula, int idUsuario)
        {
            string query = @"SELECT COUNT(*) FROM Medico WHERE UPPER(Matricula) = @Matricula AND IdUsuario <> @IdUsuario";
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@Matricula", matricula.ToUpper());
                    datos.EstablecerParametros("@IdUsuario", idUsuario);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                            return lector.GetInt32(0) > 0;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error en MedicoRepositorio.ExistePorMatriculaEnOtro: " + ex.Message);
                    throw;
                }
            }
            return false;
        }

        public bool ExistePorDocumentoEnOtro(string dni, int idUsuario)
        {
            string query = @"SELECT COUNT(*) FROM Medico WHERE NumeroDocumento = @Dni AND IdUsuario <> @IdUsuario";
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@Dni", dni);
                    datos.EstablecerParametros("@IdUsuario", idUsuario);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                            return lector.GetInt32(0) > 0;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error en MedicoRepositorio.ExistePorDocumentoEnOtro: " + ex.Message);
                    throw;
                }
            }
            return false;
        }


        public Medico ObtenerPorId(int idMedico)
        {
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"
                            SELECT M.IdMedico, M.Nombre, M.Apellido, M.NumeroDocumento, M.Genero, 
                                    M.FechaNacimiento, M.Telefono, M.Email, M.Matricula, 
                                    M.IdUsuario, M.IdEspecialidad, E.Nombre AS NombreEspecialidad, M.Estado, M.FechaAlta, M.FechaModificacion
                            FROM Medico M
                            JOIN Especialidad E ON M.IdEspecialidad = E.IdEspecialidad
                            WHERE M.IdMedico @IdMedico";

                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@IdMedico", idMedico);

                try
                {
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            var medico = MedicoMapper.MapearAEntidad(lector);

                            if (!lector.IsDBNull(lector.GetOrdinal("IdEspecialidad")))
                            {
                                int idEsp = lector.GetInt32(lector.GetOrdinal("IdEspecialidad"));
                                string nombreEspecialidad = lector.GetString(lector.GetOrdinal("NombreEspecialidad"));
                                medico.Especialidades.Add(new Especialidad { IdEspecialidad = idEsp, Nombre = nombreEspecialidad });
                            }
                            return medico;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error en MedicoRepositorio.ObtenerPorUsuarioId: " + ex.Message);
                    throw;
                }
            }
            return null;
        }

    }
}
