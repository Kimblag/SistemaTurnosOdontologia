using SGTO.Dominio.Enums;
using System;


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

    }
}
