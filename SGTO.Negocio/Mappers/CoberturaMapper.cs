using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs;
using System.Collections.Generic;

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
                NombrePlanes = nombrePlanes

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
    }
}
