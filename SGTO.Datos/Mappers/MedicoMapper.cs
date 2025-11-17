using SGTO.Dominio.Entidades;
using System;
using System.Data.SqlClient;

namespace SGTO.Datos.Mappers
{
    public static class MedicoMapper
    {

        public static Medico MapearAEntidad(SqlDataReader lector, int idUsuario = 0)
        {
            Medico medico = new Medico();

            if (TieneColumna(lector, "IdMedico"))
                medico.IdMedico = lector.GetInt32(lector.GetOrdinal("IdMedico"));

            if (TieneColumna(lector, "Nombre"))
                medico.Nombre = lector.GetString(lector.GetOrdinal("Nombre"));

            if (TieneColumna(lector, "Apellido"))
                medico.Apellido = lector.GetString(lector.GetOrdinal("Apellido"));

            if (TieneColumna(lector, "NumeroDocumento"))
                medico.Dni = new Dominio.ObjetosValor.DocumentoIdentidad(lector.GetString(lector.GetOrdinal("NumeroDocumento")));

            if (TieneColumna(lector, "Genero"))
                medico.Genero = EnumeracionMapperDatos.MapearGenero(lector, "Genero");

            if (TieneColumna(lector, "FechaNacimiento"))
                medico.FechaNacimiento = lector.GetDateTime(lector.GetOrdinal("FechaNacimiento"));

            if (TieneColumna(lector, "Telefono"))
                medico.Telefono = new Dominio.ObjetosValor.Telefono(lector.GetString(lector.GetOrdinal("Telefono")));

            if (TieneColumna(lector, "Matricula"))
                medico.Matricula = lector.GetString(lector.GetOrdinal("Matricula"));

            if (TieneColumna(lector, "Estado"))
                medico.Estado = EnumeracionMapperDatos.MapearEstadoEntidad(lector, "Estado");

            if (idUsuario != 0)
                medico.Usuario = new Usuario { IdUsuario = idUsuario };

            return medico;
        }


        private static bool TieneColumna(SqlDataReader lector, string nombreColumna)
        {
            for (int i = 0; i < lector.FieldCount; i++)
            {
                if (lector.GetName(i).Equals(nombreColumna, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }


    }
}
