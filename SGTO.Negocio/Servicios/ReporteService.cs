using SGTO.Comun.DTOs;
using SGTO.Datos.Repositorios;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Medicos;
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
        private readonly EspecialidadRepositorio _repositorioEspecialidad;

        public ReporteService()
        {
            _repositorioReportes = new ReporteRepositorio();
            _repositorioCobertura = new CoberturaRepositorio();
            _repositorioPlan = new PlanRepositorio();
            _repositorioEspecialidad = new EspecialidadRepositorio();
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
        public ReporteMedicosKpiDto ObtenerKpisMedicos(DateTime? fechaDesde, DateTime? fechaHasta)
        {
            try
            {
                return _repositorioReportes.ConsultarKpisMedicos(fechaDesde, fechaHasta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener KPIs de médicos.", ex);
            }
        }

        public List<ReporteMedicosDto> ObtenerReporteMedicosFiltrado(DateTime? fechaDesde, DateTime? fechaHasta, int? idEspecialidad)
        {
            try
            {
                return _repositorioReportes.ConsultarMedicosFiltrado(fechaDesde, fechaHasta, idEspecialidad);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el reporte de médicos.", ex);
            }
        }
        public List<ReporteTratamientosDto> ObtenerReporteTratamientosFiltrado(int? idEspecialidad, string estado)
        {
            try
            {
                return _repositorioReportes.ConsultarTratamientosFiltrado(idEspecialidad, estado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el reporte de tratamientos.", ex);
            }
        }

        public ReporteTratamientosKpiDto ObtenerKpisTratamientos(DateTime? fechaDesde, DateTime? fechaHasta)
        {
            try
            {
                var kpis = _repositorioReportes.ConsultarKpisTratamientos(fechaDesde, fechaHasta);
                return kpis;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener KPIs de tratamientos.", ex);
            }
        }

        public List<EspecialidadDto> ListarEspecialidades(string estado = null)
        {
            try
            {
                return EspecialidadMapper.MapearListaADto(_repositorioEspecialidad.Listar(estado));
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar especialidades.", ex);
            }
        }


        public List<ReporteTurnosDto> ObtenerReporteTurnosFiltrado(DateTime? fechaDesde, DateTime? fechaHasta, string estado, int? idMedico, int? idEspecialidad)
        {
            try
            {
                if (fechaDesde.HasValue && fechaHasta.HasValue && fechaDesde.Value > fechaHasta.Value)
                {
                    throw new ArgumentException("La fecha 'Desde' no puede ser mayor a la fecha 'Hasta'.");
                }

                return _repositorioReportes.ConsultarTurnosFiltrado(fechaDesde, fechaHasta, estado, idMedico, idEspecialidad);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el reporte detallado de turnos.", ex);
            }
        }

        public ReporteTurnosKpiDto ObtenerKpisTurnos(DateTime? fechaDesde, DateTime? fechaHasta)
        {
            try
            {
                return _repositorioReportes.ConsultarKpisTurnos(fechaDesde, fechaHasta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al calcular los indicadores de turnos.", ex);
            }
        }

        public List<ReporteCoberturasDto> ObtenerReporteCoberturas(string estado)
        {
            try
            {
                return _repositorioReportes.ConsultarReporteCoberturas(estado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el reporte de coberturas.", ex);
            }
        }

        public List<ReportePlanesDto> ObtenerReportePlanes(int? idCobertura, string estado, string orden)
        {
            try
            {
                return _repositorioReportes.ConsultarReportePlanes(idCobertura, estado, orden);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el reporte de planes.", ex);
            }
        }


    }
}
