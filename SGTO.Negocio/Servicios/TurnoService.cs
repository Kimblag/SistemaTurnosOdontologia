using SGTO.Datos.Repositorios;
using System;
using System.Collections.Generic;


namespace SGTO.Negocio.Servicios
{
    public class TurnoService
    {
        private readonly TurnoRepositorio _turnoRepositorio;

        public TurnoService()
        {
            _turnoRepositorio = new TurnoRepositorio();
        }

        public bool TieneTurnosActivosPorCobertura(int idCobertura)
        {
            try
            {
                return _turnoRepositorio.ExisteTurnoActivoPorCobertura(idCobertura);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool TieneTurnosActivosPorPlan(int idPlan)
        {
            try
            {
                return _turnoRepositorio.ExisteTurnoActivoPorPlan(idPlan);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool TieneTurnosActivosPorEspecialidad(int idEspecialidad)
        {
            try
            {
                return _turnoRepositorio.ExisteTurnoActivoPorEspecialidad(idEspecialidad);
                   
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool TieneTurnosActivosPorTratamiento(int idTratamiento)
        {
            try
            {
                return _turnoRepositorio.ExisteTurnoActivoPorEspecialidad(idTratamiento);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
