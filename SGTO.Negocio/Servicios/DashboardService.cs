using SGTO.Datos.Repositorios;
using System;
using System.Collections.Generic;
using SGTO.Comun.DTOs;
using System.Diagnostics;

namespace SGTO.Negocio.Servicios
{
    public class DashboardService
    {

        private readonly DashboardRepositorio _repositorioDashboard;

        public DashboardService()
        {
            _repositorioDashboard = new DashboardRepositorio();
        }


        public DashboardResumenDto ObtenerResumenDiario()
        {
            try
            {
                DashboardResumenDto resumen = _repositorioDashboard.ObtenerResumenDiario();
                return resumen;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error en resumen diario: " + ex.Message);
                throw;
            }
        }

        public List<DashboardActividadSemanalDto> ObtenerActividadSemanal()
        {
            try
            {
                List<DashboardActividadSemanalDto> lista = _repositorioDashboard.ObtenerActividadSemanal();
                return lista;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error en actividad semanal: " + ex.Message);
                throw;
            }
        }

    }
}
