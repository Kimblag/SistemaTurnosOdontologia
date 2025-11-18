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
                // Obtener datos del Medico 
                var medicoEntidad = _repositorioMedico.ObtenerPorId(id);
                if (medicoEntidad == null) return null;

                // Obtener historial de turnos
                var turnoRepo = new TurnoRepositorio(); // Instancia directa o inyección
                var historial = turnoRepo.ObtenerHistorialPorMedico(id);

                var dto = new MedicoDetalleDto
                {
                    IdMedico = medicoEntidad.IdMedico,
                    NombreCompleto = $"{medicoEntidad.Nombre} {medicoEntidad.Apellido}",
                    NumeroDocumento = medicoEntidad.Dni != null ? medicoEntidad.Dni.Numero : "-",
                    FechaNacimiento = medicoEntidad.FechaNacimiento,
                    Telefono = medicoEntidad.Telefono != null ? medicoEntidad.Telefono.Numero : "-",
                    Email = medicoEntidad.Email != null ? medicoEntidad.Email.Valor : "-",
                    Estado = medicoEntidad.Estado.ToString(),


                    // Info Profesional
                    Matricula = medicoEntidad.Matricula,
                    FechaIncorporacion = medicoEntidad.FechaAlta,
                    Especialidades = medicoEntidad.Especialidades.Select(e => e.Nombre).ToList(),

                    // Lógica de Negocio: Calculamos estadísticas basadas en el historial
                    CantidadPacientesAtendidos = historial.Select(x => x.Paciente).Distinct().Count(),
                    CoberturasAceptadas = historial.Select(x => x.Cobertura).Distinct().ToList(),

                    // Lista para la grilla
                    HistorialTurnos = historial
                };

                return dto;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }

}