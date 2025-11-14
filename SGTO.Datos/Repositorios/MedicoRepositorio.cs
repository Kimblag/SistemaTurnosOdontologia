using SGTO.Datos.Infraestructura;
using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using System;
using System.Collections.Generic;
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


        public int Crear(Medico nuevoMedico, ConexionDBFactory datos)
        {
            string query = @"
                        INSERT INTO Medico 
                            (Nombre, Apellido, NumeroDocumento, Genero, FechaNacimiento, Telefono, 
                                Matricula, IdUsuario, Estado, FechaAlta, FechaModificacion)
                        OUTPUT INSERTED.IdMedico
                        VALUES
                            (@Nombre, @Apellido, @NumeroDocumento, @Genero, @FechaNacimiento, @Telefono, 
                                @Matricula, @IdUsuario, @Estado, @FechaAlta, @FechaModificacion)";

            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@Nombre", nuevoMedico.Nombre);
            datos.EstablecerParametros("@Apellido", nuevoMedico.Apellido);
            datos.EstablecerParametros("@NumeroDocumento", nuevoMedico.Dni.Numero);
            datos.EstablecerParametros("@Genero", nuevoMedico.Genero.ToString()[0]);
            datos.EstablecerParametros("@FechaNacimiento", nuevoMedico.FechaNacimiento);
            datos.EstablecerParametros("@Telefono", nuevoMedico.Telefono.Numero);
            datos.EstablecerParametros("@Matricula", nuevoMedico.Matricula);
            datos.EstablecerParametros("@IdUsuario", nuevoMedico.Usuario.IdUsuario);
            datos.EstablecerParametros("@Estado", nuevoMedico.Estado.ToString()[0]);
            datos.EstablecerParametros("@FechaAlta", nuevoMedico.FechaAlta);
            datos.EstablecerParametros("@FechaModificacion", nuevoMedico.FechaModificacion);

            try
            {
                object resultado = datos.EjecutarAccionEscalar();
                return Convert.ToInt32(resultado);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error en MedicoRepositorio.Crear: " + ex.Message);
                throw;
            }
        }


        public void CrearEspecialidadMedico(int idMedico, int idEspecialidad, ConexionDBFactory datos)
        {
            string query = @"
                        INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad)
                            VALUES (@IdMedico, @IdEspecialidad)";

            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@IdMedico", idMedico);
            datos.EstablecerParametros("@IdEspecialidad", idEspecialidad);
            datos.EjecutarAccion();
        }


        public Medico ObtenerPorUsuarioId(int idUsuario)
        {
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"
                                SELECT M.IdMedico, M.Nombre, M.Apellido, M.NumeroDocumento, M.Genero, 
                                        M.FechaNacimiento, M.Telefono, M.Matricula, 
                                        M.IdUsuario, ME.IdEspecialidad, E.Nombre AS NombreEspecialidad, M.Estado, M.FechaAlta, M.FechaModificacion
                                FROM Medico M
                                JOIN MedicoEspecialidad ME ON ME.IdMedico = M.IdMedico
                                JOIN Especialidad E ON ME.IdEspecialidad = E.IdEspecialidad
                                WHERE M.IdUsuario = @IdUsuario";

                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@IdUsuario", idUsuario);

                try
                {
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        Medico medico = null;
                        while (lector.Read())
                        {
                            if (medico == null)
                            {
                                medico = MedicoMapper.MapearAEntidad(lector, idUsuario);
                                medico.Especialidades = new List<Especialidad>();
                            }

                            if (!lector.IsDBNull(lector.GetOrdinal("IdEspecialidad")))
                            {
                                medico.Especialidades.Add(new Especialidad
                                {
                                    IdEspecialidad = lector.GetInt32(lector.GetOrdinal("IdEspecialidad")),
                                    Nombre = lector.GetString(lector.GetOrdinal("NombreEspecialidad"))
                                });
                            }
                        }
                        return medico;
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
                    Matricula = @Matricula,
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
            datos.EstablecerParametros("@Matricula", medico.Matricula);
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
                                    M.FechaNacimiento, M.Telefono, M.Matricula, 
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

        public List<Medico> Listar(string estado = null)
        {
            List<Medico> medicos = new List<Medico>();
            Dictionary<int, Medico> mapa = new Dictionary<int, Medico>();

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"
                            SELECT 
                                M.IdMedico,
                                M.Nombre,
                                M.Apellido,
                                M.NumeroDocumento,
                                M.Genero,
                                M.FechaNacimiento,
                                M.Telefono,
                                M.Matricula,
                                M.Estado,
                                ME.IdEspecialidad,
                                E.Nombre AS NombreEspecialidad
                            FROM Medico M
                            LEFT JOIN MedicoEspecialidad ME ON M.IdMedico = ME.IdMedico
                            LEFT JOIN Especialidad E ON ME.IdEspecialidad = E.IdEspecialidad
                            {{WHERE}}
                            ORDER BY M.Apellido, M.Nombre;
                        ";

                query = estado != null
                    ? query.Replace("{{WHERE}}", $"WHERE M.Estado = '{estado[0]}'")
                    : query.Replace("{{WHERE}}", "");

                datos.DefinirConsulta(query);

                using (SqlDataReader lector = datos.EjecutarConsulta())
                {
                    while (lector.Read())
                    {
                        int idMedico = lector.GetInt32(lector.GetOrdinal("IdMedico"));

                        if (!mapa.ContainsKey(idMedico))
                        {
                            Medico medico = MedicoMapper.MapearAEntidad(lector);
                            medico.Especialidades = new List<Especialidad>();
                            mapa.Add(idMedico, medico);
                        }

                        if (!lector.IsDBNull(lector.GetOrdinal("IdEspecialidad")))
                        {
                            mapa[idMedico].Especialidades.Add(new Especialidad
                            {
                                IdEspecialidad = lector.GetInt32(lector.GetOrdinal("IdEspecialidad")),
                                Nombre = lector.GetString(lector.GetOrdinal("NombreEspecialidad"))
                            });
                        }
                    }
                }
            }

            return new List<Medico>(mapa.Values);
        }


        public void EliminarEspecialidadesDeMedico(int idMedico, ConexionDBFactory datos)
        {
            string query = "DELETE FROM MedicoEspecialidad WHERE IdMedico = @IdMedico";

            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@IdMedico", idMedico);
            datos.EjecutarAccion();
        }

    }
}
