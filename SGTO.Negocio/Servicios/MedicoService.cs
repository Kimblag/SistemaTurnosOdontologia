using SGTO.Comun.Validacion;
using SGTO.Datos.Repositorios;
using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using SGTO.Negocio.DTOs.Medicos;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGTO.Negocio.Servicios
{
    public class MedicoService
    {

        private readonly MedicoRepositorio _repositorioMedico;

        public MedicoService()
        {
            _repositorioMedico = new MedicoRepositorio();
        }

        public List<MedicoListadoDto> Listar(string estado = null)
        {
            try
            {
                return MedicoMapper.MapearAListado(_repositorioMedico.Listar(estado));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<MedicoListadoDto> ListarPorEspecialidad(int idEspecialidad)
        {
            try
            {
                return MedicoMapper.MapearAListado(_repositorioMedico.ListarPorEspecialidad(idEspecialidad));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MedicoDetalleDto ObtenerDetalle(int id)
        {
            try
            {
                var medicoEntidad = _repositorioMedico.ObtenerPorId(id);
                if (medicoEntidad == null) return null;

                var turnoRepo = new TurnoRepositorio();
                var historial = turnoRepo.ObtenerHistorialPorMedico(id);
                var dto = MedicoMapper.MapearADetalleDto(medicoEntidad);

                dto.CantidadPacientesAtendidos = historial.Select(x => x.Paciente).Distinct().Count();
                dto.CoberturasAceptadas = historial.Select(x => x.Cobertura).Distinct().ToList();
                dto.HistorialTurnos = historial;

                return dto;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }

}