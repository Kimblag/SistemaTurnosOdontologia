using SGTO.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Datos.Mappers
{
    public static class EnumeracionMapper
    {
        public static EstadoEntidad MapearEstadoEntidad(SqlDataReader lector, string nombreColumna)
        {
            char valor = lector.GetString(lector.GetOrdinal(nombreColumna))[0];
            return valor == 'A' ? EstadoEntidad.Activo : EstadoEntidad.Inactivo;
        }

        public static char ObtenerChar(EstadoEntidad estado)
        {
            return estado == EstadoEntidad.Activo ? 'A' : 'I';
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
                    throw new ArgumentOutOfRangeException(
                    $"Valor desconocido '{valor}' para EstadoTurno.");
            }

        }

        public static char ObtenerChar(EstadoTurno estado)
        {
            return (char)estado;
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
                    throw new ArgumentOutOfRangeException(
                        $"Valor desconocido '{valor}' para Genero.");
            }
        }

        public static char ObtenerChar(Genero genero)
        {
            return (char)genero;
        }
    }
}
