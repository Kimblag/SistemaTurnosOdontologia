using SGTO.Datos.Infraestructura;
using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SGTO.Comun.DTOs;


namespace SGTO.Datos.Repositorios
{
    public class TurnoRepositorio
    {

        public bool ExisteTurnoActivoPorEspecialidad(int idEspecialidad)
        {
            bool resultado = false;
            string query = @"SELECT COUNT(*)
                    FROM Turno
                    WHERE IdEspecialidad = @IdEspecialidad
                      AND Estado NOT IN ('C', 'Z', 'X')";
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                datos.LimpiarParametros();
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@idEspecialidad", idEspecialidad);

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
        public bool ExisteTurnoActivoPorCobertura(int idCobertura)
        {
            bool resultado = false;

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"SELECT COUNT(*)
                                FROM Turno
                            WHERE IdCobertura = @IdCobertura
                                    AND Estado NOT IN ('C', 'Z', 'X')";
                datos.LimpiarParametros();
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@IdCobertura", idCobertura);

                using (SqlDataReader lector = datos.EjecutarConsulta())
                {
                    try
                    {
                        if (lector.Read())
                        {
                            int cantidad = lector.GetInt32(0);
                            resultado = cantidad > 0;
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return resultado;
        }

        public bool ExisteTurnoActivoPorPlan(int idPlan)
        {
            bool resultado = false;

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"SELECT COUNT(*)
                                    FROM Turno
                                WHERE IdPlan = @IdPlan
                                    AND Estado NOT IN ('C', 'Z', 'X')";
                datos.LimpiarParametros();
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@IdPlan", idPlan);

                using (SqlDataReader lector = datos.EjecutarConsulta())
                {
                    try
                    {
                        if (lector.Read())
                        {
                            int cantidad = lector.GetInt32(0);
                            resultado = cantidad > 0;
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return resultado;
        }

        public bool ExisteTurnoActivoPorPaciente(int idPaciente)
        {
            bool resultado = false;
            string query = @"SELECT COUNT(*)
                         FROM Turno
                         WHERE IdPaciente = @IdPaciente
                           AND Estado NOT IN ('C', 'Z', 'X')";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                datos.LimpiarParametros();
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@IdPaciente", idPaciente);

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


        public List<Turno> ListaPorPaciente(int idPaciente)
        {
            List<Turno> turnos = new List<Turno>();

            string query = @"
                            SELECT 
                                T.IdTurno,
                                T.FechaInicio,
                                T.FechaFin,
                                T.Estado AS EstadoTurno,
                                T.Observaciones,
                                M.IdMedico,
                                M.Nombre AS NombreMedico,
                                M.Apellido AS ApellidoMedico,
                                E.IdEspecialidad,
                                E.Nombre AS NombreEspecialidad
                            FROM Turno T
                                INNER JOIN Medico M ON T.IdMedico = M.IdMedico
                                INNER JOIN Especialidad E ON T.IdEspecialidad = E.IdEspecialidad
                            WHERE T.IdPaciente = @IdPaciente
                            ORDER BY T.FechaInicio DESC";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                datos.LimpiarParametros();
                datos.EstablecerParametros("@IdPaciente", idPaciente);
                datos.DefinirConsulta(query);
                try
                {
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        while (lector.Read())
                        {
                            Turno turno = TurnoMapper.MapearAEntidadBasico(lector);
                            turnos.Add(turno);
                        }
                    }
                    return turnos;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        public List<Turno> Listar()
        {
            List<Turno> turnos = new List<Turno>();

            string query = @"
                SELECT 
                    T.IdTurno,
                    T.FechaInicio,
                    T.FechaFin,
                    T.Estado AS EstadoTurno,
                    T.Observaciones,

                    P.IdPaciente,
                    P.Nombre AS NombrePaciente,
                    P.Apellido AS ApellidoPaciente,

                    M.IdMedico,
                    M.Nombre AS NombreMedico,
                    M.Apellido AS ApellidoMedico,

                    E.IdEspecialidad,
                    E.Nombre AS NombreEspecialidad,

                    C.IdCobertura,
                    C.Nombre AS NombreCobertura,

                    PL.IdPlan,
                    PL.Nombre AS NombrePlan

                FROM Turno T
                    INNER JOIN Paciente P ON T.IdPaciente = P.IdPaciente
                    INNER JOIN Medico M ON T.IdMedico = M.IdMedico
                    INNER JOIN Especialidad E ON T.IdEspecialidad = E.IdEspecialidad
                    INNER JOIN Cobertura C ON T.IdCobertura = C.IdCobertura
                    LEFT JOIN [Plan] PL ON T.IdPlan = PL.IdPlan
                ORDER BY T.FechaInicio DESC";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                datos.DefinirConsulta(query);
                try
                {
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        while (lector.Read())
                        {
                            Turno turno = TurnoMapper.MapearAEntidadListado(lector);
                            turnos.Add(turno);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return turnos;
        }


        public List<Turno> ObtenerTurnosPorMedicoEnRango(int idMedico, DateTime desde, DateTime hasta)
        {
            List<Turno> turnosMedico = new List<Turno>();
            string query = @"
                            SELECT 
                                IdTurno,
                                IdPaciente,
                                IdMedico,
                                IdEspecialidad,
                                FechaInicio,
                                FechaFin,
                                Estado,
                                Observaciones
                            FROM Turno
                            WHERE IdMedico = @IdMedico
                              AND FechaInicio >= @Desde
                              AND FechaInicio < @Hasta
                              AND Estado NOT IN ('C', 'X', 'Z')";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                datos.LimpiarParametros();
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@IdMedico", idMedico);
                datos.EstablecerParametros("@Desde", desde);
                datos.EstablecerParametros("@Hasta", hasta);

                try
                {
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        while (lector.Read())
                        {
                            Turno turno = TurnoMapper.MapearAEntidadBasico(lector);
                            turnosMedico.Add(turno);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return turnosMedico;
            }
        }


        public int Crear(Turno turno)
        {
            string query = @"
                    INSERT INTO Turno (
                        IdPaciente,
                        IdMedico,
                        IdEspecialidad,
                        IdCobertura,
                        IdPlan,
                        FechaInicio,
                        FechaFin,
                        Estado,
                        Observaciones
                    )
                    VALUES (
                        @IdPaciente,
                        @IdMedico,
                        @IdEspecialidad,
                        @IdCobertura,
                        @IdPlan,
                        @FechaInicio,
                        @FechaFin,
                        @Estado,
                        @Observaciones
                    );

                    SELECT SCOPE_IDENTITY();
                ";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                datos.LimpiarParametros();
                datos.DefinirConsulta(query);

                datos.EstablecerParametros("@IdPaciente", turno.Paciente.IdPaciente);
                datos.EstablecerParametros("@IdMedico", turno.Medico.IdMedico);
                datos.EstablecerParametros("@IdEspecialidad", turno.Especialidad.IdEspecialidad);
                datos.EstablecerParametros("@IdCobertura", turno.Cobertura.IdCobertura);

                if (turno.Plan != null)
                    datos.EstablecerParametros("@IdPlan", turno.Plan.IdPlan);
                else
                    datos.EstablecerParametros("@IdPlan", DBNull.Value);

                datos.EstablecerParametros("@FechaInicio", turno.Horario.Inicio);
                datos.EstablecerParametros("@FechaFin", turno.Horario.Fin);
                datos.EstablecerParametros("@Estado", turno.Estado.ToString()[0]);

                if (string.IsNullOrWhiteSpace(turno.Observaciones))
                    datos.EstablecerParametros("@Observaciones", DBNull.Value);
                else
                    datos.EstablecerParametros("@Observaciones", turno.Observaciones);

                try
                {
                    int resultado = datos.EjecutarAccionEscalar();
                    return resultado;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public List<TurnoHistorialDto> ObtenerHistorialPorMedico(int idMedico)
        {
            var lista = new List<TurnoHistorialDto>();

            string query = @"
                SELECT 
                    T.FechaInicio,
                    P.Nombre + ' ' + P.Apellido AS PacienteNombre,
                    TR.Nombre AS TratamientoNombre,
                    C.Nombre AS CoberturaNombre,
                    T.Estado
                FROM Turno T
                INNER JOIN Paciente P ON T.IdPaciente = P.IdPaciente
                INNER JOIN Tratamiento TR ON T.IdTratamiento = TR.IdTratamiento
                INNER JOIN Cobertura C ON T.IdCobertura = C.IdCobertura
                WHERE T.IdMedico = @IdMedico
                ORDER BY T.FechaInicio DESC";

            using (var datos = new ConexionDBFactory())
            {
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@IdMedico", idMedico);

                using (var lector = datos.EjecutarConsulta())
                {
                    while (lector.Read())
                    {
                        var fechaInicio = lector.GetDateTime(lector.GetOrdinal("FechaInicio"));

                        string estadoCodigo = lector.GetString(lector.GetOrdinal("Estado"));
                        string estadoTexto = string.Empty;

                        switch (estadoCodigo)
                        {
                            case "N": estadoTexto = "Nuevo"; break;
                            case "P": estadoTexto = "Pendiente"; break;
                            case "R": estadoTexto = "Reprogramado"; break;
                            case "X": estadoTexto = "Ausente"; break; 
                            case "C": estadoTexto = "Cancelado"; break;
                            case "Z": estadoTexto = "Cerrado"; break; 
                            default: estadoTexto = estadoCodigo; break;
                        }

                        lista.Add(new TurnoHistorialDto
                        {
                            Fecha = fechaInicio.ToShortDateString(),
                            Hora = fechaInicio.ToString("HH:mm"),
                            Paciente = lector.GetString(lector.GetOrdinal("PacienteNombre")),
                            Tratamiento = lector.GetString(lector.GetOrdinal("TratamientoNombre")),
                            Cobertura = lector.GetString(lector.GetOrdinal("CoberturaNombre")),

                            Estado = estadoTexto
                        });
                    }
                }
            }
            return lista;
        }


        public Turno ObtenerPorId(int idTurno)
        {
            string query = @"
                   SELECT T.IdTurno,
                           T.Estado AS EstadoTurno,
                           T.FechaInicio,
                           T.FechaFin,
                           PAC.IdPaciente,
                           PAC.Nombre AS NombrePaciente,
                           PAC.Apellido AS ApellidoPaciente,
                           PAC.NumeroDocumento,
                           M.IdMedico,
                           M.Nombre AS NombreMedico,
                           M.Apellido AS ApellidoMedico,
                           E.IdEspecialidad,
                           E.Nombre AS NombreEspecialidad,
                           C.IdCobertura,
                           C.Nombre AS NombreCobertura,
                           PL.IdPlan,
                           PL.Nombre AS NombrePlan
                        FROM Turno T
                        INNER JOIN Paciente PAC ON PAC.IdPaciente = T.IdPaciente
                        INNER JOIN Medico M ON M.IdMedico = T.IdMedico
                        INNER JOIN Especialidad E ON E.IdEspecialidad = T.IdEspecialidad
                        INNER JOIN Cobertura C ON C.IdCobertura = T.IdCobertura
                        LEFT JOIN [Plan] PL ON PL.IdCobertura = C.IdCobertura
                    WHERE T.IdTurno = @IdTurno";

            Turno turno = null;
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                datos.LimpiarParametros();
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@IdTurno", idTurno);

                try
                {
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        if (lector.Read())
                        {
                            turno = TurnoMapper.MapearAEntidadCompleto(lector);
                        }
                    }

                }
                catch (Exception)
                {

                    throw;
                }
            }
            return turno;
        }


        public void Actualizar(Turno turno, int idUsuarioModificacion = 0)
        {
            if (turno == null) throw new ArgumentNullException(nameof(turno));

            string query = @"
                        UPDATE Turno
                        SET
                            IdPaciente = @IdPaciente,
                            IdMedico = @IdMedico,
                            IdEspecialidad = @IdEspecialidad,
                            IdCobertura = @IdCobertura,
                            IdPlan = @IdPlan,
                            FechaInicio = @FechaInicio,
                            FechaFin = @FechaFin,
                            Estado = @Estado,
                            Observaciones = @Observaciones,
                            IdUsuarioModificacion = @IdUsuarioModificacion,
                            FechaModificacion = GETDATE()
                        WHERE IdTurno = @IdTurno";

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                datos.LimpiarParametros();
                datos.DefinirConsulta(query);

                datos.EstablecerParametros("@IdTurno", turno.IdTurno);
                datos.EstablecerParametros("@IdPaciente", turno.Paciente.IdPaciente);
                datos.EstablecerParametros("@IdMedico", turno.Medico.IdMedico);
                datos.EstablecerParametros("@IdEspecialidad", turno.Especialidad.IdEspecialidad);
                datos.EstablecerParametros("@IdCobertura", turno.Cobertura.IdCobertura);
                datos.EstablecerParametros("@IdPlan", (object)turno.Plan?.IdPlan ?? DBNull.Value);
                datos.EstablecerParametros("@FechaInicio", turno.Horario.Inicio);
                datos.EstablecerParametros("@FechaFin", turno.Horario.Fin);
                datos.EstablecerParametros("@Estado", turno.Estado.ToString()[0]);
                datos.EstablecerParametros("@Observaciones", (object)turno.Observaciones ?? DBNull.Value);
                if (idUsuarioModificacion != 0)
                    datos.EstablecerParametros("@IdUsuarioModificacion", idUsuarioModificacion);
                else
                    datos.EstablecerParametros("@IdUsuarioModificacion", DBNull.Value);


                try
                {
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