using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums; 
using SGTO.Negocio.DTOs;
using System.Collections.Generic;
using SGTO.Datos.Mappers; 

namespace SGTO.Negocio.Mappers
{
    public static class EspecialidadMapper
    {
        public static EspecialidadDto MapearADto(Especialidad especialidad)
        {
            List<string> nombreTratamientos = new List<string>();

            if (especialidad.TratamientosAsociados != null && especialidad.TratamientosAsociados.Count > 0)
            {
                foreach (Tratamiento tratamiento in especialidad.TratamientosAsociados)
                {
                    nombreTratamientos.Add(tratamiento.Nombre);
                }
            }

            EspecialidadDto especialidadDto = new EspecialidadDto()
            {
                IdEspecialidad = especialidad.IdEspecialidad,
                Nombre = especialidad.Nombre,
                Descripcion = especialidad.Descripcion,
                CantidadTratamientos = (especialidad.TratamientosAsociados != null) ? especialidad.TratamientosAsociados.Count : 0,
                Estado = especialidad.Estado.ToString(),
                NombreTratamientos = nombreTratamientos
            };

            return especialidadDto;
        }

        public static EspecialidadDto MapearADto(int idEspecialidad, string nombre, string descripcion, string estado)
        {
            EspecialidadDto especialidadDto = new EspecialidadDto()
            {
                IdEspecialidad = idEspecialidad,
                Nombre = nombre,
                Descripcion = descripcion,
                CantidadTratamientos = 0, 
                Estado = estado,
                NombreTratamientos = new List<string>()
            };
            return especialidadDto;
        }

        public static List<EspecialidadDto> MapearListaADto(List<Especialidad> especialidades)
        {
            List<EspecialidadDto> especialidadesDtos = new List<EspecialidadDto>();
            foreach (Especialidad especialidad in especialidades)
            {
                especialidadesDtos.Add(MapearADto(especialidad));
            }
            return especialidadesDtos;
        }

        public static Especialidad MapearAEntidad(EspecialidadDto especialidadDto)
        {
          
            EstadoEntidad estado = EnumeracionMapperNegocio.MapearEstadoEntidad(especialidadDto.Estado);

            List<Tratamiento> tratamientosAsociados = new List<Tratamiento>();

            Especialidad especialidad = new Especialidad(
                especialidadDto.IdEspecialidad,
                especialidadDto.Nombre,
                especialidadDto.Descripcion,
                tratamientosAsociados, // La lista vacía
                estado
            );

            return especialidad;
        }
    }
}