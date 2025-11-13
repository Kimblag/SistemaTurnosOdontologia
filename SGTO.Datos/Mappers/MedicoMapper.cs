using SGTO.Dominio.Entidades;
using System.Data.SqlClient;

namespace SGTO.Datos.Mappers
{
    public static class MedicoMapper
    {

        public static Medico MapearAEntidad(SqlDataReader lector, int idUsuario = 0)
        {
            var medico = new Medico
            {
                IdMedico = lector.GetInt32(lector.GetOrdinal("IdMedico")),
                Nombre = lector["Nombre"].ToString(),
                Apellido = lector["Apellido"].ToString(),
                Dni = new Dominio.ObjetosValor.DocumentoIdentidad(lector["NumeroDocumento"].ToString()),
                Genero = EnumeracionMapperDatos.MapearGenero(lector, "Genero"),
                FechaNacimiento = lector.GetDateTime(lector.GetOrdinal("FechaNacimiento")),
                Telefono = new Dominio.ObjetosValor.Telefono(lector["Telefono"].ToString()),
                Email = new Dominio.ObjetosValor.Email(lector["Email"].ToString()),
                Matricula = lector["Matricula"].ToString(),
                Estado = EnumeracionMapperDatos.MapearEstadoEntidad(lector, "Estado"),
            };

            if (idUsuario != 0)
                medico.Usuario = new Usuario { IdUsuario = idUsuario };

            return medico;
        }

    }   

}
