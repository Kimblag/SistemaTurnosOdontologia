using SGTO.Datos.Repositorios;
using SGTO.Dominio.Enums;
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


    }
}
