using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs;
using System.Collections.Generic;
using SGTO.Datos.Mappers;

namespace SGTO.Negocio.Mappers
{
    public static class CoberturaMapper
    {
        public static CoberturaDto MapearADto(Cobertura cobertura)
        {
            List<string> nombrePlanes = new List<string>();

            if (cobertura.Planes.Count > 0)
            {
                foreach (Plan plan in cobertura.Planes)
                {
                    nombrePlanes.Add(plan.Nombre);
                }
            }

            CoberturaDto coberturaDto = new CoberturaDto()
            {
                IdCobertura = cobertura.IdCobertura,
                Nombre = cobertura.Nombre,
                Descripcion = cobertura.Descripcion,
                CantidadPlanes = cobertura.Planes.Count,
                Estado = cobertura.Estado.ToString(),
                NombrePlanes = nombrePlanes,
                PorcentajeCobertura = cobertura.PorcentajeCobertura

            };

            return coberturaDto;
        }

        public static CoberturaDto MapearADto(int idCobertura, string nombre, string descripcion, string estado, decimal? porcentajeCobertura = null)
        {
            List<string> planes = new List<string>();

            CoberturaDto coberturaDto = new CoberturaDto()
            {
                IdCobertura = idCobertura,
                Nombre = nombre,
                Descripcion = descripcion,
                CantidadPlanes = 0,
                Estado = estado,
                NombrePlanes = planes,
                PorcentajeCobertura = porcentajeCobertura

            };
            return coberturaDto;
        }

        public static List<CoberturaDto> MapearListaADto(List<Cobertura> coberturas)
        {
            List<CoberturaDto> coberturasDtos = new List<CoberturaDto>();
            foreach (Cobertura cobertura in coberturas)
            {
                coberturasDtos.Add(MapearADto(cobertura));
            }
            return coberturasDtos;
        }

        public static Cobertura MapearAEntidad(CoberturaDto coberturaDto)
        {
            Cobertura cobertura = new Cobertura
            {
                IdCobertura = coberturaDto.IdCobertura,
                Nombre = coberturaDto.Nombre,
                Descripcion = coberturaDto.Descripcion,
                Planes = new List<Plan>(),
                PorcentajeCobertura = coberturaDto.PorcentajeCobertura,
                Estado = EnumeracionMapper.MapearEstadoEntidad(coberturaDto.Estado)
            };

            return cobertura;
        }



    }
}
