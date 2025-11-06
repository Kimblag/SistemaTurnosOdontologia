using SGTO.Datos.Repositorios;
using SGTO.Dominio.Enums;
using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SGTO.Negocio.Servicios
{
    public class PlanService
    {
        private readonly PlanRepositorio _repositorioPlan;

        public PlanService()
        {
            _repositorioPlan = new PlanRepositorio();
        }

        public List<PlanDto> Listar(string estado = null)
        {
            try
            {
                return PlanMapper.MapearListaADto(_repositorioPlan.Listar(estado));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DarDeBajaPlan(int idPlan, TurnoService servicioTurno)
        {
            // si tiene turnos activos no podemos dar de baja.
            if (servicioTurno.TieneTurnosActivosPorPlan(idPlan))
                throw new ExcepcionReglaNegocio("No se puede dar de baja el plan porque tiene turnos activos.");

            // si ya está dado de baja, no puede volver a darse de baja
            if (_repositorioPlan.EstadoDadoDeBaja(idPlan))
                throw new ExcepcionReglaNegocio("El plan ya se encuentra dado de baja.");

            try
            {
                _repositorioPlan.DarDeBaja(idPlan, EstadoEntidad.Inactivo.ToString()[0]);
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Error al intentar dar de baja el plan.");
            }
        }


        public PlanDto ObtenerPlanPorId(int idPlan)
        {
            try
            {
                return PlanMapper.MapearADto(_repositorioPlan.ObtenerPlanPorId(idPlan));
            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool CrearPlan(PlanDto nuevoPlanDto)
        {
            if (_repositorioPlan.ExistePlan(nuevoPlanDto.Nombre, nuevoPlanDto.IdCobertura))
            {
                throw new ExcepcionReglaNegocio($"Ya existe un plan con el nombre indicado: {nuevoPlanDto.Nombre}");
            }

            try
            {
                Cobertura cobertura = new Cobertura()
                {
                    IdCobertura = nuevoPlanDto.IdCobertura
                };
                Plan nuevoPlan = PlanMapper.MapearAEntidad(nuevoPlanDto, cobertura);
                _repositorioPlan.Crear(nuevoPlan);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool ModificarPlan(PlanDto planDto, TurnoService servicioTurno)
        {
            Cobertura cobertura = new Cobertura() { IdCobertura = planDto.IdCobertura };
            Plan planModificado = PlanMapper.MapearAEntidad(planDto, cobertura);

            Plan planActual = _repositorioPlan.ObtenerPlanPorId(planDto.IdPlan);

            bool nombreCambiado = !string.Equals(planModificado.Nombre.Trim(), planActual.Nombre.Trim(), StringComparison.OrdinalIgnoreCase);

            if (nombreCambiado && _repositorioPlan.ExistePlan(planModificado.Nombre, cobertura.IdCobertura))
            {
                throw new ExcepcionReglaNegocio($"Ya existe un plan con el nombre indicado: {planModificado.Nombre}");
            }

            bool seIntentaDarDeBaja =
                planActual.Estado == EstadoEntidad.Activo &&
                planModificado.Estado == EstadoEntidad.Inactivo;

            if (seIntentaDarDeBaja && servicioTurno.TieneTurnosActivosPorPlan(planModificado.IdPlan))
            {
                throw new ExcepcionReglaNegocio("No se puede dar de baja el plan porque tiene turnos activos.");
            }

            try
            {
                _repositorioPlan.Modificar(planModificado);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}
