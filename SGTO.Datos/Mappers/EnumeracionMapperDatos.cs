using SGTO.Dominio.Enums;
using System;
using System.Data.SqlClient;

namespace SGTO.Datos.Mappers
{
    public static class EnumeracionMapperDatos
    {
        public static EstadoEntidad MapearEstadoEntidad(SqlDataReader lector, string nombreColumna)
        {
            // metodo para mapear el estado que viene desde la base de datos en forma 'A' o 'I'
            char valor = lector.GetString(lector.GetOrdinal(nombreColumna))[0];
            return valor == 'A' ? EstadoEntidad.Activo : EstadoEntidad.Inactivo;
        }

        public static EstadoTurno MapearEstadoTurno(SqlDataReader lector, string nombreColumna)
        {
            char valor = lector.GetString(lector.GetOrdinal(nombreColumna))[0];

            switch (valor)
            {
                case 'N':
                    return EstadoTurno.Nuevo;
                case 'R':
                    return EstadoTurno.Reprogramado;
                case 'C':
                    return EstadoTurno.Cancelado;
                case 'X':
                    return EstadoTurno.NoAsistio;
                case 'Z':
                    return EstadoTurno.Cerrado;
                default:
                    throw new ArgumentOutOfRangeException($"Valor desconocido '{valor}' para EstadoTurno.");
            }
        }

        public static Genero MapearGenero(SqlDataReader lector, string nombreColumna)
        {
            char valor = lector.GetString(lector.GetOrdinal(nombreColumna))[0];
            switch (valor)
            {
                case 'M':
                    return Genero.Masculino;
                case 'F':
                    return Genero.Femenino;
                case 'O':
                    return Genero.Otro;
                case 'N':
                    return Genero.PrefiereNoDecir;
                default:
                    throw new ArgumentOutOfRangeException($"Valor desconocido '{valor}' para Genero.");
            }
        }


        public static Modulo MapearModulo(SqlDataReader lector, string nombreColumna)
        {
            string valor = lector.GetString(lector.GetOrdinal(nombreColumna));
            return (Modulo)Enum.Parse(typeof(Modulo), valor, true);
        }

        public static TipoAccion MapearTipoAccion(SqlDataReader lector, string nombreColumna)
        {
            string valor = lector.GetString(lector.GetOrdinal(nombreColumna));
            return (TipoAccion)Enum.Parse(typeof(TipoAccion), valor, true);
        }

    }
}
