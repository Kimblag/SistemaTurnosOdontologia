using SGTO.Datos.Repositorios;
using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Mappers;
using System;
using System.Collections.Generic;


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

        public List<PlanDto> ListarPorCobertura(int idCobertura, string estado = null)
        {
            try
            {
                return PlanMapper.MapearListaADto(_repositorioPlan.ListarPorCobertura(idCobertura, estado));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DarDeBaja(int idPlan, TurnoService servicioTurno)
        {
            // si tiene turnos activos no podemos dar de baja.
            if (servicioTurno.TieneTurnosActivosPorPlan(idPlan))
                throw new ExcepcionReglaNegocio("No se puede dar de baja el plan porque tiene turnos activos.");

            // si ya está dado de baja, no puede volver a darse de baja
            if (_repositorioPlan.EstaDadoDeBaja(idPlan))
                throw new ExcepcionReglaNegocio("El plan ya se encuentra dado de baja.");

            try
            {
                _repositorioPlan.ActualizarEstado(idPlan, EstadoEntidad.Inactivo.ToString()[0]);
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Error al intentar dar de baja el plan.");
            }
        }


        public PlanDto ObtenerPorId(int idPlan)
        {
            try
            {
                return PlanMapper.MapearADto(_repositorioPlan.ObtenerPorId(idPlan));
            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool Crear(PlanDto nuevoPlanDto, CoberturaService servicioCobertura)
        {
            if (_repositorioPlan.ExistePorNombre(nuevoPlanDto.Nombre, nuevoPlanDto.IdCobertura))
            {
                throw new ExcepcionReglaNegocio($"Ya existe un plan con el nombre indicado: {nuevoPlanDto.Nombre}");
            }

            if (servicioCobertura.EstaInactiva(nuevoPlanDto.IdCobertura))
            {
                throw new ExcepcionReglaNegocio("No se puede crear un plan para una cobertura inactiva.");
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


        public bool Modificar(PlanDto planDto, TurnoService servicioTurno, CoberturaService servicioCobertura)
        {
            if (planDto.IdCobertura <= 0)
                throw new ExcepcionReglaNegocio("El plan debe estar asociado a una cobertura válida.");

            Cobertura cobertura = new Cobertura() { IdCobertura = planDto.IdCobertura };
            Plan planModificado = PlanMapper.MapearAEntidad(planDto, cobertura);

            Plan planActual = _repositorioPlan.ObtenerPorId(planDto.IdPlan);

            bool nombreCambiado = !string.Equals(planModificado.Nombre.Trim(), planActual.Nombre.Trim(), StringComparison.OrdinalIgnoreCase);

            if (nombreCambiado && _repositorioPlan.ExistePorNombre(planModificado.Nombre, cobertura.IdCobertura))
            {
                throw new ExcepcionReglaNegocio($"Ya existe un plan con el nombre indicado: {planModificado.Nombre}");
            }

            bool seIntentaDarDeBaja =
                planActual.Estado == EstadoEntidad.Activo &&
                planModificado.Estado == EstadoEntidad.Inactivo;

            bool seIntentaDarDeAlta =
               planActual.Estado == EstadoEntidad.Inactivo &&
               planModificado.Estado == EstadoEntidad.Activo;

            // si tiene turnos activos no se puede dar de baja
            if (seIntentaDarDeBaja && servicioTurno.TieneTurnosActivosPorPlan(planModificado.IdPlan))
            {
                throw new ExcepcionReglaNegocio("No se puede dar de baja el plan porque tiene turnos activos.");
            }

            // validar si se intenta volver a activar un plan cuya cobertura se encuentra dada de baja: no permitir activar.
            if (seIntentaDarDeAlta && servicioCobertura.EstaInactiva(cobertura.IdCobertura))
            {
                throw new ExcepcionReglaNegocio("No se puede activar plan porque la cobertura se encuentra dada de baja.");
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
