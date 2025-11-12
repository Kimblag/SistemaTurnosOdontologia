using SGTO.Comun.DTOs;
using SGTO.Datos.Repositorios;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Mappers;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SGTO.Negocio.Servicios
{
    public class ReporteService
    {
        private readonly ReporteRepositorio _repositorioReportes;
        private readonly CoberturaRepositorio _repositorioCobertura;
        private readonly PlanRepositorio _repositorioPlan;

        public ReporteService()
        {
            _repositorioReportes = new ReporteRepositorio();
            _repositorioCobertura = new CoberturaRepositorio();
            _repositorioPlan = new PlanRepositorio();
        }

        public List<ReportePacientesDto> ObtenerReportePacientes()
        {
            try
            {
                return _repositorioReportes.ConsultarPacientes();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al obtener el reporte general de pacientes.", ex);
            }
        }

        public List<ReportePacientesDto> ObtenerReportePacientesFiltrado(DateTime? fechaDesde, DateTime? fechaHasta, int? idCobertura, int? idPlan)
        {
            try
            {
                return _repositorioReportes.ConsultarPacientesFiltrado(fechaDesde, fechaHasta, idCobertura, idPlan);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el reporte de pacientes con los filtros aplicados.", ex);
            }
        }

        public ReportePacientesKpiDto ObtenerKpisPacientes(DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            try
            {
                return _repositorioReportes.ConsultarKpisPacientes(fechaDesde, fechaHasta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los indicadores (KPIs) del reporte de pacientes.", ex);
            }
        }

        public List<CoberturaDto> ListarCoberturas(string estado = null)
        {
            try
            {
                return CoberturaMapper.MapearListaADto(_repositorioCobertura.Listar(estado));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR en ListarCoberturas: " + ex.Message);
                throw new Exception("Error al listar las coberturas activas.", ex);
            }
        }

        public List<PlanDto> ListarPlanes(string estado = null)
        {
            try
            {
                return PlanMapper.MapearListaADto(_repositorioPlan.Listar(estado));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR en ListarPlanes: " + ex.Message);
                throw new Exception("Error al listar los planes activos.", ex);
            }
        }
    }
}
