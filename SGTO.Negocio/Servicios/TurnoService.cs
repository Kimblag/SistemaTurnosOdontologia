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
    }
}
