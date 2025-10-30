using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs;
using System.Collections.Generic;

namespace SGTO.Negocio.Mappers
{
    public static class CoberturaMapper
    {
        public static CoberturaDto MapearADto(Cobertura cobertura)
        {
            CoberturaDto coberturaDto = new CoberturaDto()
            {
                IdCobertura = cobertura.IdCobertura,
                Nombre = cobertura.Nombre,
                Descripcion = cobertura.Descripcion,
                CantidadPlanes = cobertura.Planes.Count,
                Estado = cobertura.Estado.ToString()
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
