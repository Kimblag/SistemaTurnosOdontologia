using SGTO.Dominio.Enums;
using System;
using System.Collections.Generic;


namespace SGTO.Negocio.Mappers
{
    public static class EnumeracionMapperNegocio
    {

        public static EstadoEntidad MapearEstadoEntidad(string estado)
        {
            if (string.IsNullOrWhiteSpace(estado))
                throw new ArgumentException("El estado no puede ser nulo.");

            estado = estado.ToLower();

            switch (estado)
            {
                case "activo":
                case "a":
                    return EstadoEntidad.Activo;

                case "inactivo":
                case "i":
                    return EstadoEntidad.Inactivo;

                default:
                    throw new ArgumentOutOfRangeException(
                        $"Valor desconocido '{estado}' para EstadoEntidad.");
            }
        }

        public static char ObtenerChar(EstadoEntidad estado)
        {
            return estado == EstadoEntidad.Activo ? 'A' : 'I';
        }

        public static Genero MapearGenero(string genero)
        {
            if (string.IsNullOrWhiteSpace(genero))
                throw new ArgumentException("El género no puede ser nulo.");

            char valor = genero.ToUpper()[0];

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


        public static string ObtenerNombreEstadoTurno(char estado)
        {
            Dictionary<char, string> nombresEstadosTurno = new Dictionary<char, string>
            {
                { 'N', "Nuevo" },
                { 'R', "Reprogramado" },
                { 'C', "Cancelado" },
                { 'X', "No asistió" },
                { 'Z', "Cerrado" }
            };
            return nombresEstadosTurno.ContainsKey(estado) ? nombresEstadosTurno[estado] : estado.ToString();
        }

        public static char ObtenerChar(EstadoTurno estado)
        {
            switch (estado)
            {
                case EstadoTurno.Nuevo: return 'N';
                case EstadoTurno.Reprogramado: return 'R';
                case EstadoTurno.Cancelado: return 'C';
                case EstadoTurno.NoAsistio: return 'X';
                case EstadoTurno.Cerrado: return 'Z';
                default: throw new ArgumentOutOfRangeException($"Estado de turno desconocido: {estado}");
            }
        }

        public static EstadoTurno MapearEstadoTurno(char estado)
        {
            switch (estado)
            {
                case 'N': return EstadoTurno.Nuevo;
                case 'R': return EstadoTurno.Reprogramado;
                case 'C': return EstadoTurno.Cancelado;
                case 'X': return EstadoTurno.NoAsistio;
                case 'Z': return EstadoTurno.Cerrado;
                default: throw new ArgumentOutOfRangeException($"Char desconocido para EstadoTurno: {estado}");
            }
        }

    }
}
