using SGTO.Datos.Infraestructura;
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

    }
}
