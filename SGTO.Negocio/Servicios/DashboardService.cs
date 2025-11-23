using SGTO.Comun.DTOs;
using SGTO.Datos.Repositorios;
using SGTO.Negocio.DTOs.Seguridad;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SGTO.Negocio.Servicios
{
    public class DashboardService
    {

        private readonly DashboardRepositorio _repositorioDashboard;
        private readonly MedicoRepositorio _repositorioMedico;

        public DashboardService()
        {
            _repositorioDashboard = new DashboardRepositorio();
            _repositorioMedico = new MedicoRepositorio();
        }

        private int? ObtenerFiltroMedico(UsuarioSesionDto usuario)
        {
            if (usuario.NombreRol.Equals("Médico", StringComparison.OrdinalIgnoreCase))
            {
                return _repositorioMedico.ObtenerPorUsuarioId(usuario.IdUsuario).IdMedico;
            }

            return null;
        }


        public DashboardResumenDto ObtenerResumenDiario(UsuarioSesionDto usuario)
        {
            try
            {
                int? idMedico = ObtenerFiltroMedico(usuario);
                return _repositorioDashboard.ObtenerResumenDiario(idMedico);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DashboardActividadSemanalDto> ObtenerActividadSemanal(UsuarioSesionDto usuario)
        {
            try
            {
                int? idMedico = ObtenerFiltroMedico(usuario);
                return _repositorioDashboard.ObtenerActividadSemanal(idMedico);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
