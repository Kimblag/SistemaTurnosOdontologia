using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs.Medicos;
using System.Collections.Generic;

namespace SGTO.Negocio.Mappers
{
    public static class HorarioSemanalMapper
    {
        public static List<HorarioSemanalDto> MapearListaADto(List<HorarioSemanalMedico> entidades)
        {
            var lista = new List<HorarioSemanalDto>();
            if (entidades == null) return lista;

            foreach (var entidad in entidades)
            {
                lista.Add(new HorarioSemanalDto
                {
                    DiaSemana = entidad.DiaSemana,
                    HoraInicio = entidad.HoraInicio,
                    HoraFin = entidad.HoraFin,
                    Estado = entidad.Estado.ToString()
                });
            }

            return lista;
        }
    }
}
