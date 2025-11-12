using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Negocio.DTOs;
using System.Collections.Generic;


namespace SGTO.Negocio.Mappers
{
    public static class TratamientoMapper
    {
        public static TratamientoDto MapearADto(Tratamiento tratamiento)
        {
            string nombreEspecialidad = (tratamiento.Especialidad != null) ? tratamiento.Especialidad.Nombre : string.Empty;
            int idEspecialidad = (tratamiento.Especialidad != null) ? tratamiento.Especialidad.IdEspecialidad : 0;
            int cantEspecialidades = (tratamiento.Especialidad != null) ? 1 : 0;

            TratamientoDto tratamientoDto = new TratamientoDto()
            {
                IdTratamiento = tratamiento.IdTratamiento,
                Nombre = tratamiento.Nombre,
                Descripcion = tratamiento.Descripcion,
                CostoBase = tratamiento.CostoBase,
                CantidadEspecialidades = cantEspecialidades,
                Estado = tratamiento.Estado.ToString(),
                NombreEspecialidad = nombreEspecialidad,
                IdEspecialidad = idEspecialidad
            };

            return tratamientoDto;
        }

        public static List<TratamientoDto> MapearListaADto(List<Tratamiento> tratamientos)
        {
            List<TratamientoDto> tratamientosDtos = new List<TratamientoDto>();
            foreach (Tratamiento tratamiento in tratamientos)
            {
                tratamientosDtos.Add(MapearADto(tratamiento));
            }
            return tratamientosDtos;
        }

        public static Tratamiento MapearAEntidad(TratamientoDto tratamientoDto)
        {
            EstadoEntidad estado = EnumeracionMapperNegocio.MapearEstadoEntidad(tratamientoDto.Estado);

            Especialidad especialidadAsociada = new Especialidad()
            {
                IdEspecialidad = tratamientoDto.IdEspecialidad
            };

            Tratamiento tratamiento = new Tratamiento(
              tratamientoDto.IdTratamiento,
              tratamientoDto.Nombre,
              tratamientoDto.Descripcion,
              tratamientoDto.CostoBase,
              especialidadAsociada,
              estado                
            );

            return tratamiento;
        }
    }
}