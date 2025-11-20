using SGTO.Datos.Repositorios;
using SGTO.Negocio.DTOs.Medicos;
using SGTO.Negocio.Mappers;
using System;
using System.Collections.Generic;

namespace SGTO.Negocio.Servicios
{
    public class MedicoService
    {

        private readonly MedicoRepositorio _repositorioMedico;
        private readonly TurnoRepositorio _repositorioTurno;


        public MedicoService()
        {
            _repositorioMedico = new MedicoRepositorio();
            _repositorioTurno = new TurnoRepositorio();
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

                var historial = _repositorioTurno.ObtenerHistorialPorMedico(id);

                var dto = MedicoMapper.MapearADetalleDto(medicoEntidad);

                List<string> pacientesUnicos = new List<string>();
                foreach (var turno in historial)
                {
                    if (!pacientesUnicos.Contains(turno.Paciente))
                    {
                        pacientesUnicos.Add(turno.Paciente);
                    }
                }
                dto.CantidadPacientesAtendidos = pacientesUnicos.Count;

                List<string> coberturasUnicas = new List<string>();
                foreach (var turno in historial)
                {
                    if (!coberturasUnicas.Contains(turno.Cobertura))
                    {
                        coberturasUnicas.Add(turno.Cobertura);
                    }
                }
                dto.CoberturasAceptadas = coberturasUnicas;

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