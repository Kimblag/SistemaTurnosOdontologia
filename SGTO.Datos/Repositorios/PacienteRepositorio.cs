using SGTO.Datos.Infraestructura;
using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SGTO.Datos.Repositorios
{
    public class PacienteRepositorio
    {

        public List<Paciente> Listar(string estado = null)
        {
            List<Paciente> pacientes = new List<Paciente>();

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"SELECT PCTE.IdPaciente,
                                       PCTE.Apellido AS ApellidoPaciente,
                                       PCTE.Nombre AS NombrePaciente,
                                       PCTE.NumeroDocumento,
                                       PCTE.FechaNacimiento,
                                       PCTE.Genero,
                                       PCTE.Telefono,
                                       PCTE.Email,
                                       PCTE.Estado AS EstadoPaciente,
                                       C.IdCobertura,
                                       C.Nombre AS NombreCobertura,
                                       C.Descripcion AS DescripcionCobertura,
                                       C.Estado AS EstadoCobertura,
                                       P.IdPlan,
                                       P.Nombre AS NombrePlan,
                                       P.Descripcion AS DescripcionPlan,
                                       P.PorcentajeCobertura,
                                       P.Estado AS EstadoPlan
                                FROM Paciente PCTE
                                INNER JOIN Cobertura C ON PCTE.IdCobertura = C.IdCobertura
                                LEFT JOIN [Plan] P ON PCTE.IdPlan = P.IdPlan
                                {{WHERE}}
                                ORDER BY PCTE.Apellido, PCTE.Nombre";

                query = estado != null
              ? query.Replace("{{WHERE}}", $" WHERE PCTE.Estado = '{estado[0]}'")
              : query.Replace("{{WHERE}}", "");

                datos.DefinirConsulta(query);

                try
                {
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        while (lector.Read())
                        {

                            Cobertura cobertura = CoberturaMapper.MapearAEntidad(lector);

                            // el paln puede ser null
                            Plan plan = null;
                            if (!lector.IsDBNull(lector.GetOrdinal("IdPlan")))
                            {
                                plan = PlanMapper.MapearAEntidad(lector);
                            }

                            Paciente paciente = PacienteMapper.MapearAEntidad(lector);
                            paciente.Cobertura = cobertura;
                            paciente.Plan = plan;
                            pacientes.Add(paciente);
                        }
                    }
                    return pacientes;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public Paciente ObtenerPorId(int IdPaciente)
        {
            Paciente paciente = null;
            string query = @"SELECT 
                                PCTE.IdPaciente,
                                PCTE.Nombre AS NombrePaciente,
                                PCTE.Apellido AS ApellidoPaciente,
                                PCTE.NumeroDocumento,
                                PCTE.Genero,
                                PCTE.FechaNacimiento,
                                PCTE.Telefono,
                                PCTE.Email,
                                 PCTE.Estado AS EstadoPaciente,
                                C.IdCobertura, C.Nombre AS NombreCobertura, C.Descripcion AS DescripcionCobertura, C.Estado AS EstadoCobertura,
                                P.IdPlan, P.Nombre AS NombrePlan, P.Descripcion AS DescripcionPlan, P.PorcentajeCobertura, P.Estado AS EstadoPlan
                            FROM Paciente PCTE
                                INNER JOIN Cobertura C ON PCTE.IdCobertura = C.IdCobertura
                                LEFT JOIN [Plan] P ON PCTE.IdPlan = P.IdPlan
                            WHERE PCTE.IdPaciente = @IdPaciente";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                datos.LimpiarParametros();
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@IdPaciente", IdPaciente);

                try
                {
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            Cobertura cobertura = CoberturaMapper.MapearAEntidad(lector);

                            Plan plan = null;
                            if (!lector.IsDBNull(lector.GetOrdinal("IdPlan")))
                            {
                                plan = PlanMapper.MapearAEntidad(lector);
                            }
                            paciente = PacienteMapper.MapearAEntidad(lector);
                            paciente.Cobertura = cobertura;
                            paciente.Plan = plan;
                        }
                    }
                    return paciente;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        public void Crear(Paciente paciente)
        {
            string query = @"INSERT INTO Paciente 
                         (Nombre, Apellido, NumeroDocumento, Genero, FechaNacimiento, Telefono, Email, IdCobertura, IdPlan, Estado)
                         VALUES (@nombre, @apellido, @dni, @genero, @fechaNacimiento, @telefono, @email, @idCobertura, @idPlan, @estado)";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@nombre", paciente.Nombre);
                    datos.EstablecerParametros("@apellido", paciente.Apellido);
                    datos.EstablecerParametros("@dni", paciente.Dni.Numero);
                    datos.EstablecerParametros("@genero", (char)paciente.Genero);
                    datos.EstablecerParametros("@fechaNacimiento", paciente.FechaNacimiento);
                    datos.EstablecerParametros("@telefono", paciente.Telefono.Numero);
                    datos.EstablecerParametros("@email", paciente.Email.Valor);
                    datos.EstablecerParametros("@idCobertura", paciente.Cobertura.IdCobertura);
                    datos.EstablecerParametros("@idPlan", (object)paciente.Plan?.IdPlan ?? DBNull.Value);
                    datos.EstablecerParametros("@estado", paciente.Estado.ToString()[0]);

                    datos.EjecutarAccion();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }



        public void Modificar(Paciente paciente)
        {
            string query = @"UPDATE Paciente
                         SET Nombre=@nombre, Apellido=@apellido, NumeroDocumento=@dni,
                             Genero=@genero, FechaNacimiento=@fechaNacimiento, 
                             Telefono=@telefono, Email=@email, 
                             IdCobertura=@idCobertura, IdPlan=@idPlan,
                             Estado=@estado, FechaModificacion=GETDATE()
                         WHERE IdPaciente=@IdPaciente";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@nombre", paciente.Nombre);
                    datos.EstablecerParametros("@apellido", paciente.Apellido);
                    datos.EstablecerParametros("@dni", paciente.Dni.Numero);
                    datos.EstablecerParametros("@genero", (char)paciente.Genero);
                    datos.EstablecerParametros("@fechaNacimiento", paciente.FechaNacimiento);
                    datos.EstablecerParametros("@telefono", paciente.Telefono.Numero);
                    datos.EstablecerParametros("@email", paciente.Email.Valor);
                    datos.EstablecerParametros("@idCobertura", paciente.Cobertura.IdCobertura);
                    datos.EstablecerParametros("@idPlan", (object)paciente.Plan?.IdPlan ?? DBNull.Value);
                    datos.EstablecerParametros("@estado", (char)paciente.Estado);
                    datos.EstablecerParametros("@IdPaciente", paciente.IdPaciente);

                    datos.EjecutarAccion();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        public void DarDeBaja(int IdPaciente)
        {
            string query = @"UPDATE Paciente 
                         SET Estado='I', FechaModificacion=GETDATE()
                         WHERE IdPaciente=@IdPaciente";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.LimpiarParametros();
                    datos.DefinirConsulta(query);
                    datos.EstablecerParametros("@IdPaciente", IdPaciente);
                    datos.EjecutarAccion();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        public bool ExistePorId(int IdPaciente)
        {
            bool resultado = false;
            string query = @"SELECT COUNT(*) 
                                 FROM Paciente 
                                 WHERE IdPaciente = @IdPaciente";
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                datos.LimpiarParametros();
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@IdPaciente", IdPaciente);

                try
                {
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            int cantidad = lector.GetInt32(0);
                            resultado = cantidad > 0;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return resultado;
        }

        public bool ExistePorDni(string numeroDocumento)
        {
            bool resultado = false;
            string query = @"SELECT COUNT(*) 
                                 FROM Paciente 
                            WHERE NumeroDocumento = @NumeroDocumento";
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                datos.LimpiarParametros();
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@NumeroDocumento", numeroDocumento);

                try
                {
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            int cantidad = lector.GetInt32(0);
                            resultado = cantidad > 0;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return resultado;
        }


        public bool EstaDadoDeBaja(int IdPaciente)
        {
            bool resultado = false;

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"SELECT Estado 
                                 FROM Paciente 
                                 WHERE IdPaciente = @IdPaciente";

                datos.LimpiarParametros();
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@IdPaciente", IdPaciente);

                try
                {
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            char estado = Convert.ToChar(lector["Estado"]);
                            resultado = estado == 'I';
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return resultado;
        }


    }
}
