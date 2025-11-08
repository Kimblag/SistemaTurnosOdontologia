using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SGTO.Datos.Mappers
{
    public static class PacienteMapper
    {

        public static Paciente MapearAEntidad(SqlDataReader lector)
        {
            int idPaciente = lector.GetInt32(lector.GetOrdinal("IdPaciente"));
            string nombre = lector.GetString(lector.GetOrdinal("NombrePaciente"));
            string apellido = lector.GetString(lector.GetOrdinal("ApellidoPaciente"));
            string numeroDocumento = lector.GetString(lector.GetOrdinal("NumeroDocumento"));
            Genero genero = EnumeracionMapperDatos.MapearGenero(lector, "Genero");
            string telefono = lector.GetString(lector.GetOrdinal("Telefono"));
            string email = lector.GetString(lector.GetOrdinal("Email"));
            DateTime fechaNacimiento = lector.GetDateTime(lector.GetOrdinal("FechaNacimiento"));
            EstadoEntidad estado = EnumeracionMapperDatos.MapearEstadoEntidad(lector, "EstadoPaciente");

            var paciente = new Paciente()
            {
                IdPaciente = idPaciente,
                Nombre = nombre,
                Apellido = apellido,
                Dni = new DocumentoIdentidad(numeroDocumento),
                FechaNacimiento = fechaNacimiento,
                Genero = genero,
                Telefono = new Telefono(telefono),
                Email = new Email(email),
                Cobertura = null,
                Plan = null,
                Turnos = new List<Turno>(),
                HistoriaClinica = new List<HistoriaClinicaRegistro>(),
                Estado = estado
            };
            return paciente;
        }

    }
}
