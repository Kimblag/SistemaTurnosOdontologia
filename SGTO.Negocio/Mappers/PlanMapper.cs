using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Negocio.Mappers
{
    public static class PlanMapper
    {
        public static Plan MapearAEntidad(PlanDto planDto, Cobertura cobertura = null)
        {
            Plan plan = new Plan()
            {
                IdPlan = planDto.IdPlan != 0 ? planDto.IdPlan : 0,
                Nombre = planDto.Nombre,
                Descripcion = planDto.Descripcion,
                Estado = EnumeracionMapper.MapearEstadoEntidad(planDto.Estado),
                Cobertura = new Cobertura(),
                PorcentajeCobertura = planDto.PorcentajeCobertura,
            };

            if (cobertura != null)
            {
                plan.Cobertura = cobertura;
            }
            return plan;
        }

        public static List<Plan> MapearAEntidad(List<PlanDto> listaPlanDtos, Cobertura cobertura = null)
        {
            List<Plan> planes = new List<Plan>();

            foreach (PlanDto dto in listaPlanDtos)
            {
                Plan plan = new Plan()
                {
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion,
                    Estado = EnumeracionMapper.MapearEstadoEntidad(dto.Estado),
                    PorcentajeCobertura = dto.PorcentajeCobertura
                };

                if (cobertura != null)
                {
                    plan.Cobertura = cobertura;
                }

                planes.Add(plan);
            }
            return planes;
        }

        public static PlanDto MapearADto(Plan plan)
        {
            PlanDto planDto = new PlanDto()
            {
                IdPlan = plan.IdPlan,
                Nombre = plan.Nombre,
                Descripcion = plan.Descripcion,
                PorcentajeCobertura = plan.PorcentajeCobertura,
                Estado = plan.Estado.ToString(),
                IdCobertura = plan.Cobertura.IdCobertura,
                NombreCobertura = plan.Cobertura.Nombre
            };

            return planDto;
        }

        public static PlanDto MapearADto(int idPlan, string nombre,
            string descripcion, decimal porcentajeCobertura, string estado,
            int idCobertura, string nombreCobertura = null)
        {
            PlanDto planDto = new PlanDto()
            {
                IdPlan = idPlan,
                Nombre = nombre,
                Descripcion = descripcion,
                PorcentajeCobertura = porcentajeCobertura,
                Estado = estado,
                IdCobertura = idCobertura,
                NombreCobertura = nombreCobertura ?? string.Empty
            };

            return planDto;
        }

        public static List<PlanDto> MapearListaADto(List<Plan> planes)
        {
            List<PlanDto> planesDto = new List<PlanDto>();
            foreach (Plan plan in planes)
            {
                planesDto.Add(MapearADto(plan));
            }
            return planesDto;
        }

    }
}
